using CaptionMaker.Service.CaptionMaker;
using ImageMagick.Drawing;

namespace CaptionMaker.Test;

[TestClass]
public sealed class CaptionMakerTest
{
    [TestMethod]
    public void CaptionTextShorterThanTheImageWidthReturnsOneLine()
    {
        var captionMaker = new CaptionMakerService(1080);

        Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions("Short caption");

        Assert.AreEqual(captions.Count, 1);
    }

    [TestMethod]
    public void CaptionTextLongerThanTheImageWidthReturnsMultiplesLines()
    {
        var captionMaker = new CaptionMakerService(1080);

        Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed malesuada eget orci at rhoncus. Sed venenatis mattis turpis ut pretium. Nullam id faucibus neque, nec rutrum neque. Nulla id nibh sit amet.");

        Assert.IsTrue(captions.Count > 1);
    }

    [TestMethod]
    public void CaptionTextShorterThanTheImageWidthWithoutWhitespaceReturnsOneLine()
    {
        var captionMaker = new CaptionMakerService(1080);

        Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions("Loremipsumdolorsitamet");

        Assert.AreEqual(captions.Count, 1);
    }

    [TestMethod]
    public void CaptionTextLongerThanTheImageWidthWithoutWhitespaceReturnsMultiplesLines()
    {
        var captionMaker = new CaptionMakerService(1080);

        Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions("Loremipsumdolorsitamet,consecteturadipiscingelit.Sedmalesuadaegetorciatrhoncus.Sedvenenatismattisturpisutpretium.Nullamidfaucibusneque,necrutrumneque.Nullaidnibhsitamet.");

        Assert.IsTrue(captions.Count > 1);
    }
}
