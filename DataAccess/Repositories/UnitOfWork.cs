using AutoMapper;
using DataAccess_EF.Data;
using DataAccess_EF.Helpers;
using DataAccess_EF.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DataAccess_EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly JWT _jwt;

        public IDeveloperRepository Developers { get; private set; }

        public IProjectRepository Projects { get; private set; }
        public IUserRepository Users { get; private set; }
        public IAccountRepository Accounts { get; private set; }

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IMapper mapper,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _jwt = jwt.Value;
            Developers = new DeveloperRepository(_context);
            Projects = new ProjectRepository(_context);
            Users = new UserRepository(_userManager, _roleManager, _mapper);
            Accounts = new AccountRepository(_userManager, _tokenService, _signInManager);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
