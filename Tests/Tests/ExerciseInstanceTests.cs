using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExerciseInstanceTests : TestBase
    {
        private BaseService<ExerciseInstance, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new ExerciseInstanceService(GetExerciseInstanceRepository(), GetExerciseRepository());
        }

        [Test]
        public void GetAllExerciseInstances()
        {
            // Arrange
            List<ExerciseInstance> exerciseInstances = _service.Reader.GetAll();
            int count = exerciseInstances.Count;
            IDomainIdentifiable<long> createdExerciseInstance = CreateExerciseInstance();

            // Act
            exerciseInstances = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, exerciseInstances.Count);

            _service.Delete(createdExerciseInstance.Id);
        }

        [Test]
        public void CreateNewExerciseInstance()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdExerciseInstance = CreateExerciseInstance();

            // Assert
            ExerciseInstance fetchedExerciseInstance = _service.Reader.Get(createdExerciseInstance.Id);
            Assert.AreEqual(fetchedExerciseInstance.Id, createdExerciseInstance.Id);
            Assert.AreEqual(fetchedExerciseInstance.Exercise.ExRxName, "Barbell Bench Press");
            Assert.AreEqual(fetchedExerciseInstance.Set, 1);
            Assert.AreEqual(fetchedExerciseInstance.Reps, 6);
            Assert.AreEqual(fetchedExerciseInstance.Weight, 100);

            _service.Delete(createdExerciseInstance.Id);
        }

        [Test]
        public void UpdateExistingExerciseInstance()
        {
            // Arrange
            var createdExerciseInstance = (ExerciseInstance)CreateExerciseInstance();
            createdExerciseInstance.Weight = 110;

            // Act
            var updatedExerciseInstance = (ExerciseInstance)_service.Update(createdExerciseInstance);

            // Assert
            ExerciseInstance fetchedExerciseInstance = _service.Reader.Get(createdExerciseInstance.Id);
            Assert.AreEqual(fetchedExerciseInstance.Weight, updatedExerciseInstance.Weight);

            _service.Delete(createdExerciseInstance.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteExerciseInstance()
        {
            // Arrange
            IDomainIdentifiable<long> createdExerciseInstance = CreateExerciseInstance();

            // Act
            _service.Delete(createdExerciseInstance.Id);

            // Assert
            _service.Reader.Get(createdExerciseInstance.Id);
        }

        private IDomainIdentifiable<long> CreateExerciseInstance()
        {
            var exerciseInstance = new ExerciseInstance(DateTime.Now, new Exercise("Barbell Bench Press", "Barbell Flat Bench Press"), 1, 6, 100);

            IDomainIdentifiable<long> createdExerciseInstance = _service.Create(exerciseInstance);

            return createdExerciseInstance;
        }
    }
}