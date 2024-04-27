using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server_a.Data.Models;
using server_a.Data.Collections;
using Microsoft.AspNetCore.Authorization;

namespace server_a.Api
{
    [ApiController]
    public class SandwichApi(SandwichCollection sandwichCollection) : ControllerBase
    {
        private readonly SandwichCollection _sandwichCollection = sandwichCollection;

        /// <summary>
        /// Add a new sandwich to the store. Require Login
        /// </summary>
        /// <param name="sandwich">Sandwich object that needs to be added to the store</param>
        /// <response code="400">Invalid input</response>
        [HttpPost]
        [Route("/v1/sandwich")]
        [Authorize]
        public virtual IActionResult AddSandwich([FromBody] Sandwich sandwich)
        {
            sandwich.Id = _sandwichCollection.Count;
            _sandwichCollection.Add(sandwich);

            return Ok(sandwich);
        }

        /// <summary>
        /// Deletes a sandwich. Require Login
        /// </summary>
        /// <param name="sandwichId">Sandwich id to delete</param>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Sandwich not found</response>
        [HttpDelete]
        [Route("/v1/sandwich/{sandwichId}")]
        [Authorize]
        public IActionResult DeleteSandwich([FromRoute][Required] long? sandwichId)
        {
            var sandwich = _sandwichCollection.FirstOrDefault(s => s.Id == sandwichId);
            if (sandwich == null)
            {
                return NotFound();
            }

            _sandwichCollection.Remove(sandwich);
            return Ok();
        }

        /// <summary>
        /// Find sandwich by ID
        /// </summary>
        /// <remarks>Returns a single sandwich</remarks>
        /// <param name="sandwichId">ID of sandwich to return</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Sandwich not found</response>
        [HttpGet]
        [Route("/v1/sandwich/{sandwichId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Sandwich))]
        public IActionResult GetSandwichById([FromRoute][Required] long? sandwichId)
        {
            var sandwich = _sandwichCollection.FirstOrDefault(s => s.Id == sandwichId);
            if (sandwich == null)
            {
                return NotFound();
            }

            return Ok(sandwich);
        }

        /// <summary>
        /// Get a list of all sandwiches. Empty array if no sandwiches are found.
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/v1/sandwich")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Sandwich>))]
        public IActionResult GetSandwiches()
        {
            return Ok(_sandwichCollection);
        }

        /// <summary>
        /// Updates a sandwich in the store with JSON in body. Require Login
        /// </summary>
        /// <param name="sandwichId">ID of sandwich to return</param>
        /// <param name="newSandwich">Sandwich object that needs to be added to the store</param>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Sandwich not found</response>
        [HttpPut]
        [Route("/v1/sandwich/{sandwichId}")]
        [Authorize]
        public virtual IActionResult UpdateSandwich([FromRoute][Required] long? sandwichId,
         [FromBody] Sandwich newSandwich)
        {
            var sandwich = _sandwichCollection.FirstOrDefault(s => s.Id == sandwichId);
            if (sandwich == null)
            {
                return NotFound();
            }
            var id = sandwich.Id;
            newSandwich.Id = id;
            _sandwichCollection.Remove(sandwich);
            _sandwichCollection.Add(newSandwich);

            return Ok(newSandwich);
        }
    }
}
