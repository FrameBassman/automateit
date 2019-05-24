using OpenQA.Selenium;

namespace AutomateIt.SmartSelector
{
    public class Selector {
        public Scss Scss;
        public Scss FrameScss;

        public By By => Scss?.By;
        public By FrameBy => FrameScss?.By;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Selector(string scss)
            : this(ScssBuilder.Create(scss), null) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Selector(string scss, string frameScss)
            : this(ScssBuilder.Create(scss), ScssBuilder.Create(frameScss)) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Selector(Scss scss, Scss frameScss) {
            Scss = scss;
            FrameScss = frameScss;
        }
    }
}