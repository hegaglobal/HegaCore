
using UnityEngine;
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class ImageSwitcherViewModel : ObservableModel
    {
        private string m_imageName1 = "";

        [UIManAutoProperty]
        public string ImageName1
        {
            get { return this.m_imageName1; }
            set { this.m_imageName1 = value; OnPropertyChanged(nameof(this.ImageName1), value); }
        }

        private Color m_imageColor1 = Color.white;

        [UIManAutoProperty]
        public Color ImageColor1
        {
            get { return this.m_imageColor1; }
            set { this.m_imageColor1 = value; OnPropertyChanged(nameof(this.ImageColor1), value); }
        }

        private string m_imageName2 = "";

        [UIManAutoProperty]
        public string ImageName2
        {
            get { return this.m_imageName2; }
            set { this.m_imageName2 = value; OnPropertyChanged(nameof(this.ImageName2), value); }
        }

        private Color m_imageColor2 = Color.white;

        [UIManAutoProperty]
        public Color ImageColor2
        {
            get { return this.m_imageColor2; }
            set { this.m_imageColor2 = value; OnPropertyChanged(nameof(this.ImageColor2), value); }
        }
    }
}