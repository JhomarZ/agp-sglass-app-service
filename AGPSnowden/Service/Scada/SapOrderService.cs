using AGPSnowden.Model;
using AGPSnowden.Model.Scada;

namespace AGPSnowden.Service.Scada
{
    public class SapOrderService
    {

        public async Task<SapOrder> Add(SapOrder orderSap)
        {
            try
            {
                using (var context = new BdscadaEvergemContext())
                {
                    await context.SapOrders.AddAsync(orderSap);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return orderSap;
        }


    }
}
