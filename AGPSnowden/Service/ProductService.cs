using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;

namespace AGPSnowden.Service
{
    public class ProductService
    {
        public List<Product> GetAll(int Start = 0, int Records = 10, string Centro="PE02")
        {
            List<Product> list = new List<Product>();
            using (var context = new DbSnowdenContext())
            {
                //list = context.AuditSubType.ToList();
                var query = context.Products.OrderByDescending(x => x.Id)
                            .Where(x => x.Centro == Centro).AsQueryable();

                list = query.Skip(Start).Take(Records).ToList();
            }

            return list;
        }

        public async Task<Product> GetOne(int Id)
        {
            Product product = new Product();

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    product = await context.Products.FindAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return product;
        }

        public async Task<Product> Add(Product product)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    await context.Products.AddAsync(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return product;
        }

        public async Task<Product> Update(Product product)
        {

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    context.Products.Update(product);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return product;
        }

        public async Task<Product> Delete(int id)
        {
            Product product;
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    product = context.Products.Find(id);
                    product.Active = false;
                    context.Products.Update(product);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return product;
        }

    }
}
