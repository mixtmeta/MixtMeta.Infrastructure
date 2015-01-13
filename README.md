# MixtMeta.Infrastructure
Common .NET libraries for things like Entity Framework, Stripe API, etc

## Introduction

The purpose of this project is to provide some common base classes to extend from to get up and running faster. Currently 
there is support for Entity Framework 6.1 on both mySQL and SQL Server.

### Entity Framework 6

This code is targetted toward enterprise systems where a DBA will manage the SQL model and a programmer will create the EF
objects. This is _not intended_ for an environment utilizing code generation on either end.

To get up and running, subclass IMixtMetaEntity and MixtMetaEntity to start building out your EF objects.

Example User Object:

    public interface IUser : IMixtMetaEntity
    {
      int UserId { get; set; }
      string Name { get; set; }
      string Email { get; set; }
    }
  
    public class User : MixtMetaEntity, IUser
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int UserId { get; set; }
  
      [MaxLength(70)]
      public string Name { get; set; }
  
      [Required]
      [MaxLength(255)]
      [RegularExpression(@"^.+@.+\..+$")]
      public string Email { get; set; }
    }
  
Then subclass the UnitOfWork, provide your connection string and add your repositories:

    public class MyUnitOfWork : UnitOfWork
    {
      private IMixtMetaRepositoy<User> _userRepository;
      
      public IMixtMetaRepository<User> UserRepository
      {
        get { return _userRepository ?? (_userRepository = new MixtMetaRepository<User>(_context)); }
      }

      protected override string NameOrConnectionString
      {
        get { return "name=myConnectionString"; }
      }
    }
    
You can now access your data model in the following manner.

Create a user:

    using(MyUnitOfWork uow = new MyUnitOfWork()) 
    {
      IUser user = uow.UserRepository.NewObject();
      user.Name = "xxxx";
      user.Email = "xxxx@yyyy.zzz";
      uow.Save();
    }
    
Get all users:

    using(MyUnitOfWork uow = new MyUnitOfWork()) 
    {
      var users = uow.UserRepository.GetAll();
    }
