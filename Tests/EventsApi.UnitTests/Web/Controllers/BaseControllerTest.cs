﻿using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EventsApi.Core.Interfaces;
using EventsApi.Web.Profiles;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;

namespace EventsApi.UnitTests.Web.Controllers
{
    [TestFixture]
    public abstract class BaseControllerTest
    {
        protected IFixture fixture;
        protected Mock<IRepository> mockRepository;

        public BaseControllerTest()
        {
            SetupFixture();
        }

        private void SetupFixture()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            fixture.Customize<BindingInfo>(b => b.OmitAutoProperties());

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TalkProfile());
                cfg.AddProfile(new SpeakerProfile());
            });

            mockRepository = fixture.Freeze<Mock<IRepository>>();

            fixture.Inject<IMapper>(mockMapper.CreateMapper());
        }

        [TearDown]
        public void CleanUp()
        {
            mockRepository.Invocations.Clear();
        }
    }
}
