namespace SpaceTravel.Game
{
    public static class StorageService
    {
        public static bool CanConsumeInputs(StorageState storage, ResourceAmount[] inputs)
        {
            if (inputs == null || inputs.Length == 0)
                return true;

            foreach (var input in inputs)
            {
                if (input == null || input.Amount <= 0f)
                    continue;

                if (input.Resource == null)
                    continue; // or treat as invalid config

                var resourceId = input.Resource.Id;
                var current = storage.GetAmount(resourceId);

                if (current + 0.0001f < input.Amount)
                    return false;
            }

            return true;
        }

        public static void ConsumeInputs(StorageState storage, ResourceAmount[] inputs)
        {
            if (inputs == null || inputs.Length == 0)
                return;

            foreach (var input in inputs)
            {
                if (input == null || input.Amount <= 0f)
                    continue;

                if (input.Resource == null)
                    continue;

                var resourceId = input.Resource.Id;
                var current = storage.GetAmount(resourceId);
                storage.SetAmount(resourceId, current - input.Amount);
            }
        }

        public static float AddOutputs(StorageState storage, ResourceAmount[] outputs, IGameDefinitions defs)
        {
            if (outputs == null || outputs.Length == 0)
                return 0f;

            float totalStored = 0f;

            foreach (var output in outputs)
            {
                if (output == null || output.Amount <= 0f)
                    continue;

                var resDef = output.Resource;
                if (resDef == null)
                    continue;

                var group = resDef.Group;
                float capacity = storage.GetCapacity(group);
                float currentGroupAmount = GetTotalAmountForGroup(storage, group, defs);
                float free = capacity - currentGroupAmount;

                if (free <= 0f)
                    continue;

                float toStore = output.Amount;
                if (toStore > free)
                    toStore = free;

                var resourceId = resDef.Id;
                float current = storage.GetAmount(resourceId);
                storage.SetAmount(resourceId, current + toStore);

                totalStored += toStore;
            }

            return totalStored;
        }

        public static float GetTotalAmountForGroup(StorageState storage, ResourceGroup group, IGameDefinitions defs)
        {
            float sum = 0f;
            foreach (var stack in storage.Resources)
            {
                var resDef = defs.GetResourceDefinition(stack.Id);
                if (resDef != null && resDef.Group == group)
                {
                    sum += stack.Amount;
                }
            }

            return sum;
        }
    }
}
