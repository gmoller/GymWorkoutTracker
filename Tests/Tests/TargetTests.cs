using System;
using System.Collections.Generic;
using ApplicationServices;
using DomainModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TargetTests : TestBase
    {
        private BaseService<Target, long> _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new TargetService(GetTargetRepository(), GetMuscleGroupRepository());
        }

        [Test]
        public void GetAllTargets()
        {
            // Arrange
            List<Target> targets = _service.Reader.GetAll();
            int count = targets.Count;
            IDomainIdentifiable<long> createdTarget = CreateTarget();

            // Act
            targets = _service.Reader.GetAll();

            // Assert
            Assert.AreEqual(count + 1, targets.Count);

            _service.Delete(createdTarget.Id);
        }

        [Test]
        public void CreateNewTarget()
        {
            // Arrange

            // Act
            IDomainIdentifiable<long> createdTarget = CreateTarget();

            // Assert
            Target fetchedTarget = _service.Reader.Get(createdTarget.Id);
            Assert.AreEqual(fetchedTarget.Id, createdTarget.Id);
            Assert.AreEqual(fetchedTarget.Name, "Soleus");

            _service.Delete(createdTarget.Id);
        }

        [Test]
        public void UpdateExistingTarget()
        {
            // Arrange
            var createdTarget = (Target)CreateTarget();
            createdTarget.Name = "Tibialis Anterior";

            // Act
            var updatedTarget = (Target)_service.Update(createdTarget);

            // Assert
            Target fetchedTarget = _service.Reader.Get(createdTarget.Id);
            Assert.AreEqual(fetchedTarget.Name, updatedTarget.Name);

            _service.Delete(createdTarget.Id);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException), ExpectedMessage = "Data not found.")]
        public void DeleteTarget()
        {
            // Arrange
            IDomainIdentifiable<long> createdTarget = CreateTarget();

            // Act
            _service.Delete(createdTarget.Id);

            // Assert
            _service.Reader.Get(createdTarget.Id);
        }

        private IDomainIdentifiable<long> CreateTarget()
        {
            var target = new Target { Name = "Soleus", MuscleGroup = new MuscleGroup { Name = "Calves" } };

            IDomainIdentifiable<long> createdTarget = _service.Create(target);

            return createdTarget;
        }
    }
}