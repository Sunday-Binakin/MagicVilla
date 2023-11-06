using MagicVilla.Models;
using MagicVilla.Models.Dto;

namespace MagicVilla.Data;

public static class VillaStore
{
    public static List<VillaDto?> VillaList = new  List<VillaDto?>
    {
        new VillaDto { Id = 1, Name = "Pool View",Occupancy = 4,Sqft = 100},
        new VillaDto { Id = 2, Name = "Beach View", Occupancy = 5,Sqft = 200}
    };
}