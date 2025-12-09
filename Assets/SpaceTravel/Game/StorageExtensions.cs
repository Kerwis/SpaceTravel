namespace SpaceTravel.Game
{
    public static class StorageExtensions
    {
        public static float GetAmount(this StorageState storage, string id)
        {
            var stack = storage.Resources.Find(r => r.Id == id);
            return stack?.Amount ?? 0f;
        }

        public static void SetAmount(this StorageState storage, string id, float amount)
        {
            var stack = storage.Resources.Find(r => r.Id == id);
            if (stack == null)
            {
                stack = new ResourceStack { Id = id };
                storage.Resources.Add(stack);
            }

            stack.Amount = amount;
        }

        public static float GetCapacity(this StorageState storage, ResourceGroup group)
        {
            var cap = storage.Capacities.Find(c => c.Group == group);
            return cap?.Capacity ?? 0f;
        }

        public static void SetCapacity(this StorageState storage, ResourceGroup group, float capacity)
        {
            var cap = storage.Capacities.Find(c => c.Group == group);
            if (cap == null)
            {
                cap = new ResourceGroupCapacity { Group = group };
                storage.Capacities.Add(cap);
            }

            cap.Capacity = capacity;
        }
    }
}