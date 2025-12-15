namespace Study_Timeline.Logic.Domain
{
	public class Category
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		public Student Student { get; private set; }


        // Creation constructor (new category)
        public Category(int id, string name, string description)
        {
            Id = id;
            SetName(name);
            SetDescription(description);
        }

        // Hydration constructor (repository)
        public Category(int id)
        {
            Id = id;
        }

        // Domain behaviour
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            Name = name;
        }

        private void SetDescription(string description)
        {
            // our description can be empty

            Description = description;
        }
    }
}
