
namespace Tameenk.Yakeen.DAL
{
    public class ChannelDataAccess : BaseDataAccess<Channel, int>
    {
        public ChannelDataAccess() :base()
        { }

        public string GetChannelNameByID(int ID)
        {
            string channelName = Get(ID).Name;
            return channelName;
        }

        public int GetChannelExpireDateByID(int ID)
        {
            int expireDays = Get(ID).ExpireDateInDays;
            return expireDays;
        }
    }
}
