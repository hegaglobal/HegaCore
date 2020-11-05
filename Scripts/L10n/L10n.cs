using System.Collections.Generic;
using VisualNovelData.Data;

namespace HegaCore
{
    public static class L10n
    {
        public static bool IsInitialized => _language != null;

        private static readonly List<IL10n> _targets;

        private static ReadL10nData _data;
        private static ILanguage _language;

        static L10n()
        {
            _targets = new List<IL10n>();
        }

        public static void Initialize(in ReadL10nData data, ILanguage language)
        {
            _data = data;
            _language = language;
        }

        public static void Register(IL10n target)
        {
            if (target == null || _targets.Contains(target))
                return;

            _targets.Add(target);
        }

        public static void Deregister(IL10n target)
        {
            if (target == null || !_targets.Contains(target))
                return;

            _targets.Remove(target);
        }

        public static void Relocalize()
        {
            for (var i = 0; i < _targets.Count; i++)
            {
                _targets[i]?.Localize();
            }
        }

        public static string Localize(string key, bool silent = false, bool keyAsDefault = false)
        {
            if (_language == null)
            {
                UnuLogger.LogError($"{nameof(L10n)} must be initialized before using");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(key))
            {
                if (!silent)
                    UnuLogger.LogWarning($"L10n key is empty");

                return string.Empty;
            }

            var text = _data.GetText(key);

            if (text == null)
            {
                if (!silent && !keyAsDefault)
                    UnuLogger.LogWarning($"Cannot find any L10n data by key={key}");

                return keyAsDefault ? key : string.Empty;
            }

            return _data.GetContent(text.ContentId).GetLocalization(_language.GetLanguage());
        }

        public static string Localize(string key, string language, bool silent = false, bool keyAsDefault = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                if (!silent)
                    UnuLogger.LogWarning($"L10n key is empty");

                return string.Empty;
            }

            if (string.IsNullOrEmpty(language))
            {
                if (!silent)
                    UnuLogger.LogWarning($"L10n language is empty");

                return string.Empty;
            }

            var text = _data.GetText(key);

            if (text == null)
            {
                if (!silent && !keyAsDefault)
                    UnuLogger.LogWarning($"Cannot find any L10n data by key={key}");

                return keyAsDefault ? key : string.Empty;
            }

            return _data.GetContent(text.ContentId).GetLocalization(language);
        }
    }
}