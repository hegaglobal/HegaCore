using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.UI
{
    public class CharacterListModule : MonoBehaviour
    {
        public event Action<int> OnSelect;

        [SerializeField]
        private CharacterModule[] characters = null;

        public int Count => this.characters.Length;

        public int CurrentIndex { get; private set; }

        private ReadDictionary<string, int> characterMap;
        private ReadList<CharacterId> availableIds;

        public void Initialize(in ReadDictionary<string, int> characterMap, in ReadList<CharacterId> availableIds)
        {
            this.characterMap = characterMap;
            this.availableIds = availableIds;

            foreach (var character in this.characters)
            {
                character.OnSelect += Character_OnSelect;
            }

            TryUnlockCharacters();

            this.CurrentIndex = -1;
        }

        public void Deinitialize()
        {
            foreach (var girl in this.characters)
            {
                girl.Deinitialize();
            }
        }

        private void TryUnlockCharacters()
        {
            foreach (var character in this.characters)
            {
                var id = this.characterMap[character.Character];
                var unlocked = this.availableIds.Contains(new CharacterId(id, 0));
                character.Initialize(unlocked);
            }
        }

        public bool IsUnlock(int index)
            => index >= 0 && index < this.characters.Length && this.characters[index].DataInstance.Unlocked;

        public int GetCharacterId(int index)
        {
            if (index < 0 || index >= this.characters.Length)
                return 0;

            var character = this.characters[index].Character;
            return this.characterMap[character];
        }

        public void SetSelected(int characterId)
        {
            for (var i = 0; i < this.characters.Length; i++)
            {
                var module = this.characters[i];
                var id = this.characterMap[module.Character];

                if (id == characterId)
                {
                    module.SetSelected(true);
                    this.CurrentIndex = i;
                }
                else
                {
                    module.SetSelected(false);
                }
            }
        }

        private void Character_OnSelect(string character)
            => this.OnSelect?.Invoke(this.characterMap[character]);
    }
}
