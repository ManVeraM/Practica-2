using System.ComponentModel.DataAnnotations;

namespace PRACTICA2.Models;
public class Reserved{


    

    public int UserId {get; set;}

    public int BookId {get; set;}


    [Key]
    public DateTime ReservedAt{get;set;}

}