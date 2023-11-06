using System.Security.Cryptography;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController:ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDto?>> GetVillaDtos()
    {
        return Ok(VillaStore.VillaList);
    }

    [HttpGet("{id:int}",Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VillaDto?> GetVilla(int id)
    {
        if (id==0)
        {
            return BadRequest();
        }

        var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
        if (villa==null)
        {
            return NotFound();
        }
        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto? villaDto)
    {
        if (VillaStore.VillaList.FirstOrDefault(u=>u.Name.ToLower()==villaDto.Name.ToLower())!=null)
        {
            ModelState.AddModelError("CustomError","Villa already Exists!!");
        }
        if (villaDto == null)
        {
            return BadRequest(villaDto);
        }

        if (villaDto.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        villaDto.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        VillaStore.VillaList.Add(villaDto);
        return CreatedAtRoute("GetVilla",new {id=villaDto.Id},villaDto);
    }

    [HttpDelete("{i:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        VillaStore.VillaList.Remove(villa);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDto? villaDTo)
    {
        if (villaDTo == null || id != villaDTo.Id)
        {
            return BadRequest();
        }

        var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
        villa.Name = villaDTo.Name;
        villa.Sqft = villaDTo.Sqft;
        villa.Occupancy = villaDTo.Occupancy;
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto>? patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }
        patchDto.ApplyTo(villa,ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}