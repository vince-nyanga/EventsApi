using System;
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
    public abstract class BaseTalksControllerTest
    {
        protected IFixture fixture;
        protected Mock<IRepository> mockRepository;

        public BaseTalksControllerTest()
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
            });

            mockRepository = fixture.Freeze<Mock<IRepository>>();

            fixture.Inject<IMapper>(mockMapper.CreateMapper());
        }
    }
}
