﻿using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System;

namespace AdminSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PersonManager _personmanager;
        private readonly FacilityManager _facilitymanager;


        /// <summary>
        /// PersonManager class instance is injected into the IndexModel class using ASP.NET Core's dependency injection system.
        /// ILogger<IndexModel> and PersonManager are constructor parameters. 
        /// These parameters represent the dependencies that the IndexModel class needs. 
        /// When an instance of IndexModel is created, 
        /// the ASP.NET Core framework automatically provides implementations for these dependencies.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="personManager"></param>
        public IndexModel(ILogger<IndexModel> logger, PersonManager personManager, FacilityManager facilitymanager)
        {
            /* ILogger<IndexModel>:
             * ILogger<T> is a built-in logging interface provided by ASP.NET Core. 
             * It allows classes to log information, warnings, errors, etc. 
             * Injecting ILogger<IndexModel> allows the IndexModel class to perform logging operations.
             */
            _logger = logger;

            /*  PersonManager:
             *  PersonManager is a custom class, likely used for managing user data, such as retrieving, adding, or updating user information in a database. 
             *  By injecting an instance of PersonManager, the IndexModel class can interact with the user data through the methods provided by PersonManager.
            */
            _personmanager = personManager;
            _facilitymanager = facilitymanager;

            /* Add the PersonManager to the service collection in program.cs
            * AddSingleton is used to register PersonManager, meaning that
            * a single instance of PersonManager will be used throughout the application.
            * */


        }

        // Property to hold the list of persons
        public List<Person> AllPersons { get; set; }
        public List<Facility> AllFacilities { get; set; } = new List<Facility>();
        public void OnGet()
        {
            //add all persons, only once
            //_personmanager.AddPerson("mm@sffp.com", "murice mcdonell", "admin1234");
            //_personmanager.AddPerson("nb@sffp.com", "nicola bart", "admin1234");
            //_personmanager.AddPerson("kb@sffp.com", "kalle brian", "admin1234");
            //_personmanager.AddPerson("st@sffp.com", "sirene torvaldsen", "admin1234");
            //_personmanager.AddPerson("as@sffp.com", "anne sofie", "admin1234");
            //_personmanager.AddPerson("oj@sffp.com", "ole jensen", "admin1234");
            //_personmanager.AddPerson("aa@sffp.com", "arne andersen", "admin1234");
            //_personmanager.AddPerson("am@sffp.com", "andré mignola", "admin1234");
            //_personmanager.AddPerson("hh@sffp.com", "hugh hugo", "admin1234");
            //_personmanager.AddPerson("es@sffp.com", "elvira strøm", "admin1234");


            //_personmanager.AddPerson("ak@sffp.com", "ahmed koch", "user1234");
            //_personmanager.AddPerson("cr@sffp.com", "cecille ravn", "user1234");
            //_personmanager.AddPerson("tr@sffp.com", "thor rasmussen", "user1234");
            //_personmanager.AddPerson("jule1998@peepquack.com", "julia tréudo", "user1234");
            //_personmanager.AddPerson("9guns@peepquack.co.uk", "tom o conner", "user1234");
            //_personmanager.AddPerson("amkrish@spaghetti.com", "amanda krishna", "user1234");
            //_personmanager.AddPerson("jacknotblack@peepquack.com", "jack white", "user1234");
            //_personmanager.AddPerson("dog4life21@spaghetti.com", "peter shoemaker", "user1234");
            //_personmanager.AddPerson("nineknives@peepquack.com", "natasha sowell", "user1234");
            //_personmanager.AddPerson("stoneman@spaghetti.com", "rune stone", "user1234");
            //_personmanager.AddPerson("hansen2003@peepquack.com", "troels hansen", "user1234");
            //_personmanager.AddPerson("monamona@peepquack.co.uk", "mona rygaard", "user1234");
            //_personmanager.AddPerson("bothemanwiththeplan2@spaghetti.com", "bob mcfarlane", "user1234");
            //_personmanager.AddPerson("jeti@peepquack.com", "jytte jettesen", "user1234");
            //_personmanager.AddPerson("margv2@spaghetti.com", "majbritt margrethe", "user1234");
            //_personmanager.AddPerson("metalangel1994@peepquack.co.uk", "austin clarke", "user1234");
            //_personmanager.AddPerson("toriyama@spaghetti.com", "akira fuglebjerg", "user1234");
            //_personmanager.AddPerson("theuffemeister@peepquack.com", "uffe lund", "user1234");
            //_personmanager.AddPerson("tageeinar1953@peepquack.com", "tage einar", "user1234");
            //_personmanager.AddPerson("mexicanfood7@spaghetti.com", "juan diego", "user1234");
            //_personmanager.AddPerson("painbread@spaghetti.com", "francisco lemuré", "user1234");
            //_personmanager.AddPerson("bluerose@spaghetti.com", "jõsé miguel", "user1234");
            //_personmanager.AddPerson("saviourboy@peepquack.co.uk", "roberto jesus", "user1234");
            //_personmanager.AddPerson("dreimanxx@peepquack.com", "steve dreiman", "user1234");
            //_personmanager.AddPerson("buddymclean@peepquack.com", "don holly", "user1234");
            //_personmanager.AddPerson("mayflowerpassion@spaghetti.com", "mary mayweather", "user1234");
            //_personmanager.AddPerson("razzer95@spaghetti.com", "rasmus shoemaker", "user1234");
            //_personmanager.AddPerson("jones2000@peepquack.co.uk", "jones o conner", "user1234");
            //_personmanager.AddPerson("everloving@spaghetti.com", "eve adamska", "user1234");
            //_personmanager.AddPerson("emmahuus91@peepquack.com", "emma huus", "user1234");
            //_personmanager.AddPerson("stine92@spaghetti.com", "stine josefinesen", "user1234");
            //_personmanager.AddPerson("victoryforme@peepquack.com", "viktor sejer", "user1234");
            //_personmanager.AddPerson("mannyboy@spaghetti.com", "kasper manning", "user1234");
            //_personmanager.AddPerson("sherlockholmes12@spaghetti.com", "patrick holmes", "user1234");
            //_personmanager.AddPerson("skovgaardvej38@spaghetti.com", "jan kristiansen", "user1234");
            //_personmanager.AddPerson("thorvaldsen3@spaghetti.com", "helmut thorvaldsen", "user1234");
            //_personmanager.AddPerson("kittyluver@peepquack.com", "emil potter", "user1234");
            //_personmanager.AddPerson("musikworld@peepquack.com", "henrik holmgaard", "user1234");
            //_personmanager.AddPerson("felicityfeline@peepquack.com", "felicity charles", "user1234");
            //_personmanager.AddPerson("baobao@spaghetti.com", "tianzhou ming", "user1234");
            //_personmanager.AddPerson("xiexie@peepquack.com", "xiao-li cheng", "user1234");

            //Show table of all persons
            //AllPersons = _personmanager.GetAllPersons();

            AllFacilities = _facilitymanager.GetAllFacilities();

        }
    }
}