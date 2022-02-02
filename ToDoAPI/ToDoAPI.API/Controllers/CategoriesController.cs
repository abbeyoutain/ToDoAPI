using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models; //added for access to DTO's
using ToDoAPI.DATA.EF; //added for access to EF
using System.Web.Http.Cors; //added for access to modify the CORS for this controller specifically

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET
        //api/categories
        public IHttpActionResult GetCategories()
        {
            List<CategoryViewModel> cats = db.Categories.Select(x => new CategoryViewModel()
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
            {
                return NotFound();
            }

            return Ok(cats);
        }//end GetCategories

        //GET
        //api/categories/id
        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(x => x.CategoryId == id).Select(x => new CategoryViewModel()
            {
                //Copy the assignments from above
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description
            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }//end GetCategory

        //POST
        //api/categories/ (HttpPost)
        public IHttpActionResult PostCategory(Category cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();
        }//end PostCategory

        //PUT
        //api/categories/ (HttpPut)
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category existingCat = db.Categories.Where(x => x.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                existingCat.CategoryId = cat.CategoryId;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutCategory

        //DELETE
        //api/categories/id (HttpDelete)
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end DeleteCategory

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }//end class
}//end namespace
