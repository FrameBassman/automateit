﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AutomateIt.Extensions.Extensions;

namespace AutomateIt.Framework.Page
{
	public class UriMatcher
	{
		private readonly string _pageAbsolutePath;
		private readonly Dictionary<string, string> _pageData;
		private readonly Dictionary<string, string> _pageParams;

		public UriMatcher(string pageAbsolutePath, Dictionary<string, string> pageData, Dictionary<string, string> pageParams)
		{
			_pageAbsolutePath = pageAbsolutePath;
			_pageData = pageData;
			_pageParams = pageParams;
		}

		public UriMatchResult Match(Uri uri, string siteAbsolutePath)
		{
			var realPath = uri.AbsolutePath.Substring(siteAbsolutePath.Length);

			var pageArr = _pageAbsolutePath.Split('/');
			var realArr = realPath.Split('/');
			if (pageArr.Length != realArr.Length)
				return UriMatchResult.Unmatched();

			// —равнить адрес страницы
			var actualData = new Dictionary<string, string>();
			for (var i = 0; i < pageArr.Length; i++)
			{
				var pageArrItem = pageArr[i];
				var realArrItem = realArr[i];
				if (pageArrItem.StartsWith("{") && pageArrItem.EndsWith("}"))
				{
					var paramName = pageArrItem.Substring(1, pageArrItem.Length - 2);
					actualData[paramName] = realArrItem;
				}
				else if (string.Compare(pageArrItem, realArrItem, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return UriMatchResult.Unmatched();
				}
			}

			// »звлечь список параметров
			var actualParams = new Dictionary<string, string>();
			var queryParamsArr = uri.Query.CutFirst('?').Split('&');
			foreach (var queryParam in queryParamsArr)
			{
				var keyvalue = queryParam.Split('=');
				if (keyvalue.Length < 2)
					continue;
				actualParams.Add(keyvalue[0], keyvalue[1]);
			}

			// —равнение Data
			if (_pageData != null)
				foreach (var key in _pageData.Keys)
					if (!actualData.ContainsKey(key) ||
					    string.Compare(actualData[key], _pageData[key], StringComparison.OrdinalIgnoreCase) != 0)
						return UriMatchResult.Unmatched();

			// —равнение Params
			if (_pageParams != null)
				foreach (string key in _pageParams.Keys)
					if (!actualParams.ContainsKey(key) ||
					    string.Compare(actualParams[key], _pageParams[key], StringComparison.OrdinalIgnoreCase) != 0)
						return UriMatchResult.Unmatched();

			return new UriMatchResult(true, actualData, actualParams);
		}
	}
}