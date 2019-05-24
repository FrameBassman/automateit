namespace AutomateIt.Framework.Browser
{
    public static class BrowserTimeouts {
        /// <summary>
        ///     Иногда после выполнения некоторого действия страница подвисает и просто ничего не происходит.
        ///     Данное значение определяет максимальное время ожидания пока отрабатывает Java Script
        /// </summary>
        public const int JS = 10;

        /// <summary>
        ///     Таймаут ожидания при поиске элемента по умолчанию
        /// </summary>
        public const int FIND = 10;

        /// <summary>
        ///     Таймаут ожидания пока отображается прогресс
        /// </summary>
        public const int AJAX = 45;

        public const int REDIRECT = 45;
        public const int WINDOW = 45;

        /// <summary>
        ///     Таймаут ожидания пока подгружаются компоненты страницы
        /// </summary>
        public const int PAGE_LOAD = 45;

        /// <summary>
        ///     Таймаут ожидания пока загрузится файл
        /// </summary>
        public const int FILE_DOWNLOAD = 45;

        /// <summary>
        ///     Timeout of waiting when file starts downloading
        /// </summary>
        public const int FILE_DOWNLOAD_START = 45;
    }
}