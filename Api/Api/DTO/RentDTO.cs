﻿using RentApi.Infrastructure.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApi.Api.DTO
{
    public class RentDTO
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public ICollection<int> EquipmentIds { get; set; }

        public static RentDTO FromEntity(Rent entity)
        {
            return new RentDTO
            {
                Id = entity.Id,
                From = entity.From,
                To = entity.To,
                EquipmentIds = entity.RentEquipment.Select(x => x.Id).ToArray()
            };
        }
    }
}