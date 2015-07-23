using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MuscleTests : TestBase
    {
        private BaseService<Muscle, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MuscleService(GetMuscleRepository(), GetMuscleGroupRepository());
        }

        [Test]
        public void GetAllMuscles()
        {
            // Arrange
            List<Muscle> muscles = _service.Reader.GetAll();
            int count = muscles.Count;
            IDomainIdentifiable<long> createdMuscle = CreateMuscle();

            // Act
            muscles = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, muscles.Count);

            _service.Delete(createdMuscle.Id);
        }

        [Test]
        public void CreateNewMuscle()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdMuscle = CreateMuscle();

            // Assert
            Muscle fetchedMuscle = _service.Reader.Get(createdMuscle.Id);
            Assert.AreEqual(fetchedMuscle.Id, createdMuscle.Id);
            Assert.AreEqual(fetchedMuscle.Name, "Soleus");

            _service.Delete(createdMuscle.Id);
        }

        [Test]
        public void UpdateExistingMuscle()
        {
            // Arrange
            var createdMuscle = (Muscle)CreateMuscle();
            createdMuscle.Name = "Tibialis Anterior";

            // Act
            var updatedMuscle = (Muscle)_service.Update(createdMuscle);

            // Assert
            Muscle fetchedMuscle = _service.Reader.Get(createdMuscle.Id);
            Assert.AreEqual(fetchedMuscle.Name, updatedMuscle.Name);

            _service.Delete(createdMuscle.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteMuscle()
        {
            // Arrange
            IDomainIdentifiable<long> createdMuscle = CreateMuscle();

            // Act
            _service.Delete(createdMuscle.Id);

            // Assert
            _service.Reader.Get(createdMuscle.Id);
        }

        private IDomainIdentifiable<long> CreateMuscle(string name = "Calves")
        {
            var muscle = new Muscle("Soleus") { BelongsToMuscleGroup = new MuscleGroup(name) };

            IDomainIdentifiable<long> createdMuscle = _service.Create(muscle);

            return createdMuscle;
        }
    }
}