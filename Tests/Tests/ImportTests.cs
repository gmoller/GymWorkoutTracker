using ApplicationServices;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ImportTests : TestBase
    {
        private IImportFromTextFileService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new ImportFromTextFileService(GetExerciseInstanceRepository(), GetExerciseRepository());
        }

        [Test]
        public void SimpleImport2()
        {
            _service.Import(@"C:\Users\Greg\Downloads\GymFrame (2).csv");
        }
    }
}