using System;

namespace AutomateIt.Framework.Browser
{
    public class BrowserOptions : ICloneable {
        public const bool FINDSINGLE_DEFAULT = true;

        public const bool WAIT_WHILE_AJAX_BEFORE_CLICK_DEFAULT = true;

        public const TypeInStyle TYPE_IN_STYLE_DEFAULT = TypeInStyle.FullValue;
        /// <summary>
        ///     Если при поиске элемента по селектору найдено несколько кидать исключение
        /// </summary>
        public bool FindSingle;

        /// <summary>
        ///     Ожидать завершения Ajax запросов перед выполнением клика
        /// </summary>
        public bool WaitWhileAjaxBeforeClick;

        public const bool USE_JS_CLICK_DEFAULT=false;

        public BrowserOptions() {
            FindSingle = FINDSINGLE_DEFAULT;
            WaitWhileAjaxBeforeClick = WAIT_WHILE_AJAX_BEFORE_CLICK_DEFAULT;
            UseJsClick = USE_JS_CLICK_DEFAULT;
            TypeInStyle = TYPE_IN_STYLE_DEFAULT;
        }

        public bool UseJsClick { get; set; }
        public TypeInStyle TypeInStyle { get; set; }

        public object Clone() {
            var options = new BrowserOptions
            {
                FindSingle = FindSingle,
                WaitWhileAjaxBeforeClick = WaitWhileAjaxBeforeClick
            };
            return options;
        }
    }
}