namespace AutomateIt.Framework.Service
{
	using System;

	public class BaseUrlInfo
	{
		public BaseUrlInfo(String subDomain, String domain, String absolutePath)
		{
			SubDomain = subDomain;
			Domain = domain;
			AbsolutePath = absolutePath;
		}

		public string SubDomain { get; private set; }
		public string Domain { get; private set; }
		public string AbsolutePath { get; private set; }

		public BaseUrlInfo ApplyActual(BaseUrlInfo baseUrlInfo)
		{
			string subDomain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.SubDomain)
				                   ? SubDomain
				                   : baseUrlInfo.SubDomain;
			string domain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.Domain)
				                ? Domain
				                : baseUrlInfo.Domain;
			string absolutePath = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.AbsolutePath)
				                      ? AbsolutePath
				                      : baseUrlInfo.AbsolutePath;
			return new BaseUrlInfo(subDomain, domain, absolutePath);
		}

		// Сформировать BaseUrl
		public String GetBaseUrl()
		{
			String s = Domain + AbsolutePath;
			if (!String.IsNullOrEmpty(SubDomain))
				s = SubDomain + "." + s;
			return s;
		}
	}
}
