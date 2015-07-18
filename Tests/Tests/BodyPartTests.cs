using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BodyPartTests : TestBase
    {
        private BaseService<BodyPart, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new BodyPartService(GetBodyPartRepository());
        }

        [Test]
        public void GetAllBodyParts()
        {
            // Arrange
            List<BodyPart> bodyParts = _service.Reader.GetAll();
            int count = bodyParts.Count;
            IDomainIdentifiable<long> createdBodyPart = CreateBodyPart();

            // Act
            bodyParts = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, bodyParts.Count);

            _service.Delete(createdBodyPart.Id);
        }

        [Test]
        public void CreateNewBodyPart()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdBodyPart = CreateBodyPart();

            // Assert
            BodyPart fetchedBodyPart = _service.Reader.Get(createdBodyPart.Id);
            Assert.AreEqual(fetchedBodyPart.Id, createdBodyPart.Id);
            Assert.AreEqual(fetchedBodyPart.Name, "Neck");

            _service.Delete(createdBodyPart.Id);
        }

        [Test]
        public void UpdateExistingBodyPart()
        {
            // Arrange
            var createdBodyPart = (BodyPart)CreateBodyPart();
            createdBodyPart.Name = "Hips";

            // Act
            var updatedBodyPart = (BodyPart)_service.Update(createdBodyPart);

            // Assert
            BodyPart fetchedBodyPart = _service.Reader.Get(createdBodyPart.Id);
            Assert.AreEqual(fetchedBodyPart.Name, updatedBodyPart.Name);

            _service.Delete(createdBodyPart.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteBodyPart()
        {
            // Arrange
            IDomainIdentifiable<long> createdBodyPart = CreateBodyPart();

            // Act
            _service.Delete(createdBodyPart.Id);

            // Assert
            _service.Reader.Get(createdBodyPart.Id);
        }

        private IDomainIdentifiable<long> CreateBodyPart()
        {
            var bodyPart = new BodyPart { Name = "Neck" };

            IDomainIdentifiable<long> createdBodyPart = _service.Create(bodyPart);

            return createdBodyPart;
        }
    }
}