﻿
using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Helper;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    public class TestBindingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult NoBinding() {

            if (Request?.Method =="POST")
            {

                var name = Request.Form["name"];
                var birthyear = Request.Form["birthyear"];
                ViewData["name"] = name;
                ViewData["birthyear"] = birthyear;
            }
            return View(); 
        }
        public IActionResult SimpleBinding(string name, string birthyear)
        {
            ViewData["name"] = name;
            ViewData["birthyear"] = birthyear;
            return View();
        }
        public IActionResult ClassBinding(UserInfor user)
        {
            var u = new UserInfor();
            if(user != null)
            {
                Mapper.PropertyCoppier<UserInfor, UserInfor>.Copy(user, u);
            }
            return View(user);
        }
        public IActionResult ComplexBinding(Person person, string[] hobby)
        {
            var u = new Person();
            if (person != null)
            {
                Mapper.PropertyCoppier<Person, Person>.Copy(person, u);
                Mapper.PropertyCoppier<Place, Place>.Copy(person.Place, u.Place);
                foreach(var h in hobby)
                {
                    var t = new Hobby
                    {
                        Name = h
                    };   
                    u.Hobby.Add(t);
                }    
                
            }
            return View(u);
        }
        public IActionResult FormCollectionBinding(IFormCollection form)
        {
            Person person = null;

            if(form!= null && form.Count>0)
            {
                Place place = new Place
                {
                    PlaceName = form["place.placename"]
                };
                List<Hobby> hobbies = new List<Hobby>();
                string hobby = form["hobby"];
                
                foreach(var st in hobby.Split(','))
                {
                    hobbies.Add(new Hobby
                    {
                        Name = st
                    });
                }
                person = new Person
                {
                    Name = form["Name"],
                    BirthYear = int.Parse(form["BirthYear"]),
                    Place  = place,
                    Hobby = hobbies
                };
            }
            else { person = new Person(); }
            return View(person);
        }
    }
}