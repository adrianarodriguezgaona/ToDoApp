using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Domain.Services;
using B4.EE.RodriguezA.ViewModels;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace B4.EE.RodriguezA.Tests
{
   public class TopicServiceTests
    {
        ReminderTopic[] testTopics;

        public TopicServiceTests()
        {
            testTopics = TestData.TestTopics;
        }

        [Fact]
        public void ReminderTopicViewModel_ReceivesLocationUser_OnInit()
        {
            //Arrange
            var reminderTopic = testTopics[0];

            var mockService = new Mock<ITopicRepository>();
            mockService.Setup(repo => repo.GetReminderTopicListById(reminderTopic.Id))
                .Returns(Task.FromResult(reminderTopic));

            //Act
            var viewmodel = new ReminderTopicViewModel(mockService.Object);

            if (viewmodel.GetType().GetMethod("Init") != null)
                viewmodel.Init(reminderTopic);

            //Assert
            Assert.NotNull(viewmodel.Name);
            Assert.Equal(viewmodel.Name, testTopics[0].Name);
        }

        [Fact]
        public async Task Delete_Returns_DeletedReminderTopicInstanceAsync()
        {
            //arrange
            var testTopic = testTopics[0];

            var mockTopicRepo = new Mock<ITopicRepository>();
            mockTopicRepo.Setup(repo => repo.DeleteReminderTopicList(testTopic.Id))
                .Returns(Task.FromResult(testTopic)); //repo returns the "fake deleted" site successfully

            var topicService = new TopicService(mockTopicRepo.Object);

            //act
            var deletedTopic = await topicService.Delete(testTopic.Id);

            //assert
            Assert.NotNull(deletedTopic);
            Assert.Equal(testTopic.Id, deletedTopic.Id);
        }

        [Fact]
        public async Task  GetAll_Returns_Results()
        {
            //arrange
            var mockTopicRepo = new Mock<ITopicRepository>();
            mockTopicRepo.Setup(repo => repo.GetReminderTopicLists())
                .Returns(Task.FromResult(testTopics.AsQueryable())); //repo returns unordered list to test

            var expectedResults = testTopics;
            var topicService = new TopicService(mockTopicRepo.Object);

            //act
            var actualResults = await topicService.GetAll();


            //assert
            var expectedTopics = expectedResults.ToArray();  //helps iterating over collection
            var actualTopics = actualResults.ToArray();      //helps iterating over collection

            // -> resulting topics 
            for (int i = 0; i < expectedTopics.Length; i++)
            {
                Assert.Equal(expectedTopics[i].Id, actualTopics[i].Id);
            }
        }

        [Fact]
        public async Task Save_Updates_When_TopicIdExists()
        {
            //arrange
            var mockTopicRepo = new Mock<ITopicRepository>();

           mockTopicRepo
                .Setup (repo => repo.GetReminderTopicListById(It.IsAny<Guid>()))
                .Returns<Guid>(id => (Task.FromResult(testTopics.FirstOrDefault(e => e.Id == id)))); //repo returns unordered list to test
            mockTopicRepo
                .Setup(e => e.AddReminderTopicList(It.IsAny<ReminderTopic>()))
                .Returns<ReminderTopic>(inputTopic => (Task.FromResult(inputTopic))); //returns same topic as input (It)
            mockTopicRepo
                .Setup(e => e.UpdateReminderTopicList(It.IsAny<ReminderTopic>()))
                .Returns<ReminderTopic>(inputTopic => (Task.FromResult(inputTopic))); //returns same topic as input (It)

            var topicService = new TopicService(mockTopicRepo.Object);

            var topicToUpdate = new ReminderTopic
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), //existing Id
                Name = "Mijn Topic",
                Items = new List<TopicItem>()
            };

            //act
            var updatedTopic = await topicService.Save(topicToUpdate);

            //assert
            mockTopicRepo.Verify(m => m.UpdateReminderTopicList(topicToUpdate), Times.AtLeastOnce());
        }

        //[Fact]
        //public async Task Save_Inserts_When_TopicIdDoesntExist()
        //{
        //    //arrange
        //    var mockTopicRepo = new Mock<ITopicRepository>();
        //    mockTopicRepo
        //        .Setup(repo => repo.GetReminderTopicListById(It.IsAny<Guid>()))
        //        .Returns<Guid>(id => (Task.FromResult(testTopics.FirstOrDefault(e => e.Id == id)))); //repo returns unordered list to test
        //    mockTopicRepo
        //        .Setup(e => e.AddReminderTopicList(It.IsAny<ReminderTopic>()))
        //        .Returns<ReminderTopic>(inputTopic => (Task.FromResult(inputTopic))); //returns same topic as input (It)
        //    mockTopicRepo
        //        .Setup(e => e.UpdateReminderTopicList(It.IsAny<ReminderTopic>()))
        //        .Returns<ReminderTopic>(inputTopic => (Task.FromResult(inputTopic))); //returns same topic as input (It)

        //    var topicService = new TopicService(mockTopicRepo.Object);

        //    var topicToInsert = new ReminderTopic
        //    {
        //        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), //non-existing Id
        //        Name = "New Topic",
        //        Items = new List<TopicItem>()
        //    };

        //    //act
        //    var insertedTopic = await topicService.Save(topicToInsert);

        //    //assert
        //    mockTopicRepo.Verify(m => m.AddReminderTopicList(topicToInsert), Times.AtLeastOnce());
        //}


        [Fact]
        public void Save_Throws_ValidationException_When_NameNotEmpty()
        {
            //arrange
            var topicService = new TopicService(null);
            var invalidTopic = new ReminderTopic
            {
                Id = Guid.NewGuid(),
                Name = null            
            };
           //act
            var saveAction = new Action(() => { topicService.Save(invalidTopic); });

            //assert
           Assert.Throws<ValidationException>(saveAction);
        }


        [Fact]
        public void Save_Throws_ValidationException_When_NameOutOfRange()
        {
            //arrange
            var topicService = new TopicService(null);
            var invalidTopicOne = new ReminderTopic
            {
                Id = Guid.NewGuid(),
                Name = "t",               
            };
            var invalidTopicTwo = new ReminderTopic
            {
                Id = Guid.NewGuid(),
                Name = "aaaaaaaaaabbbbbbbbbbddddddddddoooooooooonnnnnnnnnnjjjjj", //55 chars
               
            };

            //act
            var saveActionOne = new Action(() => { topicService.Save(invalidTopicOne); });
            var saveActionTwo = new Action(() => { topicService.Save(invalidTopicTwo); });

            //assert
            Assert.Throws<ValidationException>(saveActionOne);
            Assert.Throws<ValidationException>(saveActionTwo);
        }

       
    }
}
