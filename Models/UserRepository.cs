namespace E4UsersMVCWebApp.Models
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Data;//Add  
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Collections;
    using System.Xml.Serialization;
    using System;
    using System.Linq;

    public class UserRepository : IUserRepository
    {
        private string _filename;
        private List<UserModel> allUsers;
        private XDocument UserData;
        //Load the XML file in XmlDocument.
        private XmlDocument xmlUserdoc = new XmlDocument();

        public UserRepository(string datafilepath)
        {
            allUsers = new List<UserModel>();
            this._filename = datafilepath;
            try
            {
                //Load the XML file in XmlDocument - option 1.
                UserData = XDocument.Load(_filename);

                //Load the XML file in XmlDocument - option 2.
                xmlUserdoc = new XmlDocument();
                xmlUserdoc.Load(_filename);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public List<UserModel> GetListOfUsers()
        {
            try
            {

                //XmlNodeList elemList = xmlUserdoc.GetElementsByTagName("User");
                XmlNodeList elemList = xmlUserdoc.SelectNodes("/Users/User");
                //Loop through the selected Nodes.
                foreach (XmlNode node in elemList)
                {
                    //Fetch the Node values and assign it to Model.
                    UserModel user = new UserModel
                        {
                            Id = int.Parse(node["Id"].InnerText),
                            FirstName = node["FirstName"].InnerText,
                            LastName = node["LastName"].InnerText,
                            DoB = DateTime.Parse(node["DoB"].InnerText),
                            Cellphone = node["Cellphone"].InnerText,
                            Email = node["Email"].InnerText,
                            ImagePath = node["ImagePath"].InnerText
                        };

                    allUsers.Add(user);
                }
            }
            catch (XPathException)
            {
                throw new NotImplementedException();
            }
            return allUsers;
        }

        /// <summary>  
        /// Return list of Users from XML.  
        /// </summary>  
        /// <returns>List of Users</returns>  
        public List<UserModel> GetDataSetListOfUsers()
        {
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(_filename);
            var users = new List<UserModel>();
            users = (from cols in ds.Tables[0].AsEnumerable()
                        select new UserModel
                        {
                            Id = Convert.ToInt32(cols[0].ToString()), //Convert row to int  
                            FirstName = cols[1].ToString(),
                            LastName = cols[2].ToString(),
                            DoB = DateTime.Parse(cols[3].ToString()),
                            Cellphone = cols[4].ToString(),
                            Email = cols[5].ToString(),
                            ImagePath = cols[6].ToString()
                        }).ToList();
            return users;
        }

        public UserModel GetUserByID(int id)
        {
            return allUsers.Find(item => item.Id == id);
        }

        public bool IsExistsNameLastname(string firstname = "", string lastname = "")
        {
            List<UserModel?> users = allUsers.FindAll(t => t.FirstName == firstname && t.LastName == lastname); ;

            if (users.Count > 0)
                return true;
            else
                return false;
        }

        public void InsertUserModel(UserModel user)
        {
            user.Id = (int)(from S in UserData.Descendants("User") orderby (int)S.Element("Id") descending select (int)S.Element("Id")).FirstOrDefault() + 1;

            UserData.Root.Add(new XElement("User", new XElement("Id", user.Id),
                new XElement("FirstName", user.FirstName),
                new XElement("LastName", user.LastName),
                new XElement("DoB", user.DoB.Date.ToShortDateString()),
                new XElement("Cellphone", user.Cellphone),
                new XElement("Email", user.Email),
                new XElement("ImagePath", user.ImagePath)));
            UserData.Save(_filename);
        }

        public void EditUserModel(UserModel user)
        {
            try
            {
                XElement node = UserData.Root.Elements("User").Where(i => (int)i.Element("Id") == user.Id).FirstOrDefault();
                node.SetElementValue("FirstName", user.FirstName);
                node.SetElementValue("LastName", user.LastName);
                node.SetElementValue("DoB", user.DoB.ToShortDateString());
                node.SetElementValue("Cellphone", user.Cellphone);
                node.SetElementValue("Email", user.Email);
                node.SetElementValue("ImagePath", user.ImagePath);
                UserData.Save(_filename);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public void DeleteUserModel(int id)
        {
            try
            {
                UserData.Root.Elements("item").Where(i => (int)i.Element("Id") == id).Remove();
                UserData.Save(_filename);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

    }
}
