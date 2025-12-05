using Study_Timeline.Logic.Services;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Dependency Injection (Services)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<StudentService>();

builder.Services.AddSingleton<DbConnectionFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Session.GetInt32("StudentId") == null &&
        context.Request.Cookies.TryGetValue("StudentId", out string studentId))
    {
        if (int.TryParse(studentId, out int parsedId))
        {
            context.Session.SetInt32("StudentId", parsedId);
        }
    }

    await next.Invoke();
});

app.UseAuthorization();


app.MapRazorPages();

app.Run();
