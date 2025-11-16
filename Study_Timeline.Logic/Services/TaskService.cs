using Study_Timeline.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_Timeline.Logic.Services
{
	public class TaskService
	{
		private readonly ITaskRepository _repo;

		public TaskService(ITaskRepository repo)
		{
			_repo = repo;
		}
	}
}
