using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExerciseTests : TestBase
    {
        private BaseService<Exercise, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new ExerciseService(GetExerciseRepository(), GetMuscleRepository());
        }

        [Test]
        public void GetAllExercises()
        {
            // Arrange
            List<Exercise> exercises = _service.Reader.GetAll();
            int count = exercises.Count;
            IDomainIdentifiable<long> createdExercise = CreateExercise();

            // Act
            exercises = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, exercises.Count);

            _service.Delete(createdExercise.Id);
        }

        [Test]
        public void CreateNewExercise()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdExercise = CreateExercise();

            // Assert
            Exercise fetchedExercise = _service.Reader.Get(createdExercise.Id);
            Assert.AreEqual(fetchedExercise.Id, createdExercise.Id);
            Assert.AreEqual(fetchedExercise.ExRxName, "Barbell Decline Bench Press");

            _service.Delete(createdExercise.Id);
        }

        [Test]
        public void UpdateExistingExercise()
        {
            // Arrange
            var createdExercise = (Exercise)CreateExercise();
            createdExercise.ExRxName = "Dumbbell Decline Bench Press";

            // Act
            var updatedExercise = (Exercise)_service.Update(createdExercise);

            // Assert
            Exercise fetchedExercise = _service.Reader.Get(createdExercise.Id);
            Assert.AreEqual(fetchedExercise.ExRxName, updatedExercise.ExRxName);

            _service.Delete(createdExercise.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteExercise()
        {
            // Arrange
            IDomainIdentifiable<long> createdExercise = CreateExercise();

            // Act
            _service.Delete(createdExercise.Id);

            // Assert
            _service.Reader.Get(createdExercise.Id);
        }

        private IDomainIdentifiable<long> CreateExercise()
        {
            var exercise = new Exercise
                {
                    ExRxName = "Barbell Decline Bench Press",
                    AlternateName = "Barbell Decline Bench Press",
                    Url = "http://www.exrx.net/WeightExercises/PectoralSternal/BBDeclineBenchPress.html",
                    TargetsMuscle = new Muscle { Name = "Pectoralis Major, Sternal" }
                };

            IDomainIdentifiable<long> createdExercise = _service.Create(exercise);

            return createdExercise;
        }
    }
}