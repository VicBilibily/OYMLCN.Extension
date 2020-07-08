using OYMLCN.Extensions;
using OYMLCN.RPC.Core.RpcBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoTest.IDAL;
using DemoTest.IService;
using DemoTest.Model;
using DemoTest.Service.Filters;
using Microsoft.Extensions.Configuration;
using OYMLCN.RPC.Core;
using OYMLCN.AspNetCore.TransferJob;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using Newtonsoft.Json;

namespace DemoTest.Model
{
    public class PersonModel
    {
        [RpcProperty(Require = true)]
        public int Id { get; set; }
        public long IdCardNo { get; set; }
        [RpcProperty(Require = true, Message = "姓名不能为空")]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        [RpcProperty(DefaultValue = true)]
        public bool HasMoney { get; set; }
    }
}

namespace DemoTest.IService
{
    public interface IPersonService
    {
        PersonModel Get(int id, string token);
        List<PersonModel> GetPersons();
        bool Add(PersonModel person);
        void Delete(int id);
    }
}


namespace DemoTest.IDAL
{
    public interface IPersonDal
    {
        PersonModel Get(int id);
        List<PersonModel> GetPersons();
        bool Add(PersonModel person);
        void Delete(int id);
    }
}

namespace DemoTest.DAL
{
    public class PersonDal : IPersonDal
    {
        private List<PersonModel> persons = new List<PersonModel>();


        public bool Add(PersonModel person)
        {
            if (persons.Any(i => i.Id == person.Id))
            {
                return false;
            }
            persons.Add(person);
            return true;
        }

        public void Delete(int id)
        {
            var person = persons.FirstOrDefault(i => i.Id == id);
            if (person != null)
            {
                persons.Remove(person);
            }
        }

        public PersonModel Get(int id)
        {
            return persons.FirstOrDefault(i => i.Id == id);
        }

        public List<PersonModel> GetPersons()
        {
            return persons;
        }
    }
}


namespace DemoTest.Service.Filters
{
    public class LoggerFilter : RpcFilterAttribute
    {
        [FromServices]
        private IConfiguration Configuration { get; set; }

        public override async Task InvokeAsync(RpcContext context, RpcRequestDelegate next)
        {
            Console.WriteLine($"LoggerFilter begin,Parameters={context.Parameters.ToJsonString()}");
            await next(context);
            Console.WriteLine($"LoggerFilter end,ReturnValue={context.ReturnValue?.ToJsonString()}");
        }
    }
}
namespace DemoTest.Service2
{
    public class PersonService : IPersonService
    {
        private readonly IPersonDal _personDal;
        public PersonService(IPersonDal personDal)
        {
            _personDal = personDal;
        }

        public bool Add(PersonModel person)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PersonModel Get(int id, string token)
        {
            //return new PersonModel() { Id = id, Name = token };
            throw new NotImplementedException();
        }

        public List<PersonModel> GetPersons()
        {
            throw new NotImplementedException();
        }
    }
}
namespace DemoTest.Service
{
    public class PersonService : IPersonService
    {
        [FromServices]
        private IConfiguration Configuration { get; set; }
        [FromServices]
        private IBackgroundRunService BackgroundRunService { get; set; }

        private readonly IPersonDal _personDal;
        public PersonService(IPersonDal personDal)
        {
            _personDal = personDal;
        }

        //[LoggerFilter]
        public bool Add(PersonModel person)
        {
            return _personDal.Add(person);
        }

        public void Delete(int id)
        {
            _personDal.Delete(id);
        }

        //[LoggerFilter]
        public PersonModel Get(int id, string token = "123")
        {
            BackgroundRunService.Transfer(() => Console.WriteLine("Hello World Job0"));
            BackgroundRunService.Transfer<IWebHostEnvironment>(env => TaskJob(env));
            BackgroundRunService.Transfer<IWebHostEnvironment>(env => VoidJob(env));
            var d = _personDal.Get(id);
            if (d != null)
                d.Name = token;
            return d;
        }
        private Task TaskJob(IWebHostEnvironment logger)
        {
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("Hello World Job" + i);
                Thread.Sleep(100);
            }
            return Task.CompletedTask;
        }
        private void VoidJob(IWebHostEnvironment logger)
        {
            Console.WriteLine("Hello World Void");
        }

        public List<PersonModel> GetPersons()
        {
            return _personDal.GetPersons();
        }
    }
}