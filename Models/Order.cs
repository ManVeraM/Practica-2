using System.ComponentModel.DataAnnotations;

namespace PRACTICA2.Models;
public class Order{


    

    public int UserId {get; set;}

    public int DishId {get; set;}


    [Key]
    public DateTime OrderedAt{get;set;}


}