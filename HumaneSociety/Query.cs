using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////

        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            //if (CheckEmployeeUserNameExists)
            //    switch (crudOperation)
            //    {
            //        case "update":

            //        case "read":

            //        case "delete":

            //        case "create":

            //        default:
            //            Console.WriteLine("Not a valid query");
            //            break;
            //    }
            throw new NotImplementedException();
        }

        // Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            var animal = db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            return animal;
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal originalAnimal = null;

            try
            {
                originalAnimal = db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No animals have a AnimalId that matches the Animal passed in.");
                Console.WriteLine("No updates have been made.");
                return;
            }

            originalAnimal.Category.Name = updates[1];
            originalAnimal.Name = updates[2];
            originalAnimal.Age = Convert.ToInt32(updates[3]);
            originalAnimal.Demeanor = updates[4];
            originalAnimal.KidFriendly = bool.Parse(updates[5]);
            originalAnimal.PetFriendly = bool.Parse(updates[6]);
            originalAnimal.Weight = Convert.ToInt32(updates[7]);
            originalAnimal.AnimalId = Convert.ToInt32(updates[8]);

            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            throw new NotImplementedException();
        }
         
        // Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            int categoryID = Convert.ToInt32(db.Categories.Where(a => a.Name == categoryName));
            return categoryID;
        }
        
        internal static Room GetRoom(int animalId)
        {
            var room = db.Rooms.Where(r => r.RoomId == animalId).FirstOrDefault();
            return room;
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            var dietPlanId = db.DietPlans.Where(a => a.Name == dietPlanName).Select(a => a);
            return Convert.ToInt32(dietPlanId);
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            var adopt = db.Adoptions.Where(a => (a.ClientId == client.ClientId && a.AnimalId == animal.AnimalId)).Select(a => a).Single();
            db.Adoptions.InsertOnSubmit(adopt);
            db.SubmitChanges();
        }

        internal static List<Adoption> GetPendingAdoptions()
        {
            List<Adoption> adoptions = new List<Adoption>();
            foreach(Adoption adoption in db.Adoptions)
            {
                adoptions.Add(adoption);
            }

            return adoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            var adoption = db.Adoptions.Where(a => (a.AnimalId == animalId && a.ClientId == clientId)).FirstOrDefault();
            db.Adoptions.DeleteOnSubmit(adoption);
            db.SubmitChanges();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            var AnimalShot = db.AnimalShots.Where(a => a.AnimalId == animal.AnimalId).Select(a => a);
            return AnimalShot;
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {

            Shot shot = db.Shots.Where(s => s.Name == shotName).FirstOrDefault();
            AnimalShot shotGiven = db.AnimalShots.Where(s => s.ShotId == shot.ShotId).FirstOrDefault();

            shotGiven.DateReceived = DateTime.Now;

            animal.AnimalShots.Add(shotGiven);
        }
    }
}