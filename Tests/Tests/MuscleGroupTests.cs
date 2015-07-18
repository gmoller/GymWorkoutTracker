using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MuscleGroupTests : TestBase
    {
        private BaseService<MuscleGroup, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MuscleGroupService(GetMuscleGroupRepository());
        }

        [Test]
        public void GetAllMuscleGroups()
        {
            // Arrange
            List<MuscleGroup> muscleGroups = _service.Reader.GetAll();
            int count = muscleGroups.Count;
            IDomainIdentifiable<long> createdMuscleGroup = CreateMuscleGroup();

            // Act
            muscleGroups = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, muscleGroups.Count);

            _service.Delete(createdMuscleGroup.Id);
        }

        [Test]
        public void CreateNewMuscleGroup()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdMuscleGroup = CreateMuscleGroup();

            // Assert
            MuscleGroup fetchedMuscleGroup = _service.Reader.Get(createdMuscleGroup.Id);
            Assert.AreEqual(fetchedMuscleGroup.Id, createdMuscleGroup.Id);
            Assert.AreEqual(fetchedMuscleGroup.Name, "Neck");

            _service.Delete(createdMuscleGroup.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Unique key violation.")]
        public void CreateExistingMuscleGroup()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdMuscleGroup = CreateMuscleGroup("Chest");

            // Assert

            _service.Delete(createdMuscleGroup.Id);
        }

        [Test]
        public void UpdateExistingMuscleGroup()
        {
            // Arrange
            var createdMuscleGroup = (MuscleGroup)CreateMuscleGroup();
            createdMuscleGroup.Name = "Hips";

            // Act
            var updatedMuscleGroup = (MuscleGroup)_service.Update(createdMuscleGroup);
            updatedMuscleGroup = (MuscleGroup)_service.Update(createdMuscleGroup);

            // Assert
            MuscleGroup fetchedMuscleGroup = _service.Reader.Get(createdMuscleGroup.Id);
            Assert.AreEqual(fetchedMuscleGroup.Name, updatedMuscleGroup.Name);

            _service.Delete(createdMuscleGroup.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteMuscleGroup()
        {
            // Arrange
            IDomainIdentifiable<long> createdMuscleGroup = CreateMuscleGroup();

            // Act
            _service.Delete(createdMuscleGroup.Id);

            // Assert
            _service.Reader.Get(createdMuscleGroup.Id);
        }

        private IDomainIdentifiable<long> CreateMuscleGroup(string name = "Neck")
        {
            var muscleGroup = new MuscleGroup { Name  = name };

            IDomainIdentifiable<long> createdMuscleGroup = _service.Create(muscleGroup);

            return createdMuscleGroup;
        }
    }
}