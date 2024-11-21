﻿namespace CarModelManagementAPI.Models
{
    public class MasterData
    {
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Name { get; set; } 
        public bool IsActive { get; set; } = true; 
    }

}
