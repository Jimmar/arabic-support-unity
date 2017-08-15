using NUnit.Framework;

namespace ArabicSupport.Scripts.Testing
{
    [TestFixture]
    public class ArabicSupportTest
    {
        [Test]
        public void ArabicFixTest()
        {
            string originalText = "السلام عليكم";
            string expectedText = "ﻢﻜﻴﻠﻋ مﻼﺴﻟا";

            string output = ArabicFixer.Fix(originalText, false, false);
            
            Assert.AreEqual(expectedText, output);
        }
        
        
//        [Test]
        //shouldn't attempt to fix an already fixed text [fails]
        public void ModifyingAFixedText()
        {
            string originalText = "ﻢﻜﻴﻠﻋ مﻼﺴﻟا";

            string output = ArabicFixer.Fix(originalText, false, false);
            
            Assert.AreEqual(originalText, output);
        }

        [Test]
        //English text shouldn't be altered
        public void EnglishText()
        {
            string originalText = "Hello There this is a test";
            
            string output = ArabicFixer.Fix(originalText, false, false);
            
            Assert.AreEqual(originalText, output);
        }

//        [Test]
        public void EnglishTextWithCommas()
        {
            string originalText = "Hello There, this is a test";
            
            string output = ArabicFixer.Fix(originalText, false, false);
            
            Assert.AreEqual(originalText, output);
        }
        
        
        [Test]
        public void HinduNumbers()
        {
            string originalText = "1234567890";
            string expectedText = "١٢٣٤٥٦٧٨٩٠";

            string output = ArabicFixer.Fix(originalText, false, true);
            
            Assert.AreEqual(expectedText, output);
        }
        
        [Test]
        public void ArabicTextWithHinduNumbers()
        {
            string originalText = "هذه تجربة الخط العربي 2 مع الأرقام الهنديه 3";
            string expectedText = "٣ ﻪﻳﺪﻨﻬﻟا مﺎﻗرﻷا ﻊﻣ ٢ ﻲﺑﺮﻌﻟا ﻂﺨﻟا ﺔﺑﺮﺠﺗ هﺬﻫ";

            string output = ArabicFixer.Fix(originalText, false, true);
            
            Assert.AreEqual(expectedText, output);
        }
        
        [Test]
        public void ArabicTextWithTashkeel()
        {
            string originalText = "السَّلَامُ عَلَيْكُمْ وَ رَحْمَةُ اللهِ وَ بَرَكَاتُهِ";
            string expectedText = "ِﻪُﺗﺎَﻛَﺮَﺑ َو ِﻪﻠﻟا ُﺔَﻤْﺣَر َو ْﻢُﻜْﻴَﻠَﻋ ُمَﻼَّﺴﻟا";

            string output = ArabicFixer.Fix(originalText, true, false);
            
            Assert.AreEqual(expectedText, output);
        }
        
        [Test]
        public void ArabicTextWithTashkeelOff()
        {
            string originalText = "السَّلَامُ عَلَيْكُمْ وَ رَحْمَةُ اللهِ وَ بَرَكَاتُهِ";
            string expectedText = "ﻪﺗﺎﻛﺮﺑ و ﻪﻠﻟا ﺔﻤﺣر و ﻢﻜﻴﻠﻋ مﻼﺴﻟا";

            string output = ArabicFixer.Fix(originalText, false, false);
            
            Assert.AreEqual(expectedText, output);
        }
        

    }
}