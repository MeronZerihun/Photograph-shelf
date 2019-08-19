using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data.Common;
using System.Collections.Generic;

namespace WebApp
{
    public class Image
    {


        public int ImageId { get; set; }

        //string of the file path
        public string ImagePath{get; set;}

        //actual image from input
        [Required]
        [Display(Name = "Image File")]
        public IFormFile ImageFile{get; set;}

        [Required]
        [Display(Name = "Image Title")]
        public string Title{get; set;}

        public string Category{get; set;}
        public string Type { get; set; }

        public string UserId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description{get; set;}

        [JsonIgnore]
        public static ConnectionClass Connection = new ConnectionClass();


        public void InsertAsync(string id){
            MySqlConnection connection = Connection.CreateConnection();
            Connection.Openconnection(connection);
            var command = connection.CreateCommand() as MySqlCommand;
            command.CommandText = @"Insert into `images`(`imagePath`,`title`,`category`,`description`,`userId`,`imageType`) values(?imagePath,?title,?category,?description,?userId,?type);";
            command.Parameters.Add("?imagePath", MySqlDbType.VarChar).Value = ImagePath;
            command.Parameters.Add("?userId", MySqlDbType.VarChar).Value = id;
            command.Parameters.Add("?title", MySqlDbType.VarChar).Value = Title;
            command.Parameters.Add("?category", MySqlDbType.VarChar).Value = "None";
            command.Parameters.Add("?type", MySqlDbType.VarChar).Value = "None";
            command.Parameters.Add("?description", MySqlDbType.VarChar).Value = Description;
            int success=command.ExecuteNonQuery();
            if(success==0){
                Connection.Dispose(connection);
            }

        }
        public List<Image> SelectAsync()
        {
            MySqlConnection connection = Connection.CreateConnection();
            Connection.Openconnection(connection);
            var command = connection.CreateCommand() as MySqlCommand;
            command.CommandText = @"Select * from `images` where `category` = 'None' ";
            var images = ReadAllAsync(command.ExecuteReader());
            Connection.Dispose(connection);
            return images;

        }

        public List<Image> ReadAllAsync(DbDataReader reader){

            var images = new List<Image>();
            using (reader){
                while(reader.Read()){
                    var image = new Image()
                    {
                        ImageId = reader.GetFieldValue<int>(0),
                        ImagePath = reader.GetFieldValue<string>(1),
                        Title = reader.GetFieldValue<string>(2),
                        Category = reader.GetFieldValue<string>(3),
                        Description = reader.GetFieldValue<string>(4),
                        UserId = reader.GetFieldValue<string>(5),
                        Type = reader.GetFieldValue<string>(6)

                    };
                    images.Add(image);
                }
            }
            return images;
        }

        public void UpdateAsync(int id,string category,string type){
            MySqlConnection connection = Connection.CreateConnection();
            Connection.Openconnection(connection);
            var command = connection.CreateCommand() as MySqlCommand;
            command.CommandText = @"Update `images` set `category` = ?category, `imageType` = ?type where `imageId` = ?id ";
            command.Parameters.Add("?category", MySqlDbType.VarChar).Value = category;
            command.Parameters.Add("?id", MySqlDbType.VarChar).Value = id;
            command.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            int success = command.ExecuteNonQuery();
            if (success == 0)
            {
                Connection.Dispose(connection);
            }
        }
        public void DeleteAsync(int id){
            MySqlConnection connection = Connection.CreateConnection();
            Connection.Openconnection(connection);
            var command = connection.CreateCommand() as MySqlCommand;
            command.CommandText = @"Delete from `images` where `imageId` = ?id ";
            command.Parameters.Add("?id", MySqlDbType.VarChar).Value = id;
            int success = command.ExecuteNonQuery();
            if (success == 0)
            {
                Connection.Dispose(connection);
            }
        
        }
        public List<Image> SelectByType(string type)
        {
            MySqlConnection connection = Connection.CreateConnection();
            Connection.Openconnection(connection);
            var command = connection.CreateCommand() as MySqlCommand;
            command.CommandText = @"Select * from `images` where `category` = ?type ";
            command.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            var images = ReadAllAsync(command.ExecuteReader());
            Connection.Dispose(connection);
            return images;

        }
    }
}
