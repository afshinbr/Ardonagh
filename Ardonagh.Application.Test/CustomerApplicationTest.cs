using System.Linq.Expressions;
using Ardonagh.Application.Contracts;
using Ardonagh.Domain;
using Moq;
using Shouldly;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Ardonagh.Application.Test
{
    public class CustomerApplicationTest
    {

        private readonly List<Customer> _availableCustomers;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerApplication _customerApplication;
        public CustomerApplicationTest()
        {

            _availableCustomers = new List<Customer>()
            {
                new Customer(1, "Jack", 30, "post", 1.7),
                new Customer(2, "Alice", 29, "post", 2.1)
            };
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            // Setup a mock for Exist method in the repository to use in tests
            _customerRepositoryMock.
                Setup(x => x.Exists(It.IsAny<Expression<Func<Customer, bool>>>()))
               .Returns((Expression<Func<Customer, bool>> predicate) =>
                {
                    Func<Customer, bool> compiledPredicate = predicate.Compile();
                    Predicate<Customer> predicateDelegate = new Predicate<Customer>(compiledPredicate);
                    return _availableCustomers.Exists(predicateDelegate);
                });

            // Setup a mock for Exist GetCustomer in the repository to use in tests
            _customerRepositoryMock.
                Setup(x => x.GetCustomer(It.IsAny<long>())).
                Returns((long id) =>
                {
                    return _availableCustomers.FirstOrDefault(x => x.Id == id);
                });

            // Setup a mock for Exist GetCustomers in the repository to use in tests
            _customerRepositoryMock
                .Setup(x => x.GetCustomers())
                .Returns(_availableCustomers.OrderBy(x=>x.Id).ToList);

            // Setup a mock for Exist AddCustomer in the repository to use in tests
            _customerRepositoryMock.
                Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback((Customer customer) => _availableCustomers.Add(customer));


            _customerApplication = new CustomerApplication(_customerRepositoryMock.Object);

        }

        [Fact]
        public void Should_Return_Successful_Operation_Results_For_Creating_Valid_Request()
        {
            var createRequest = new CreateCustomer()
            {
                Name = "Tom",
                Age = 22,
                Height = 1.84,
                PostCode = "B29 5PY"
            };

            var isValid = ModelValidate(createRequest);
            var result = _customerApplication.Add(createRequest);


            isValid.ShouldBe(true);
            _customerRepositoryMock.Verify(x => x.Exists(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
            result.IsSucceeded.ShouldBe(true);
            result.Message.ShouldBe("Customer (Tom) added successfully!");
            _availableCustomers.ShouldContain(x => x.Name == "Tom");

        }

        [Fact]
        public void Should_Return_Failed_Operation_Results_For_Creating_When_User_Already_Exists()
        {
            var createRequest = new CreateCustomer()
            {
                Name = "Jack",
                Age = 30,
                Height = 1.7,
                PostCode = "post"
            };
            var isValid = ModelValidate(createRequest);
            var result = _customerApplication.Add(createRequest);

            isValid.ShouldBe(true);
            _customerRepositoryMock.Verify(x => x.Exists(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
            result.IsSucceeded.ShouldBe(false);
            result.Message.ShouldBe("This user already exists.");

        }

        [Fact]
        public void Should_Return_Successful_Operation_Results_For_Editing_Valid_Request()
        {
            var editRequest = new EditCustomer()
            {
                Id = 1,
                Name = "Tom",
                Age = 22,
                Height = 1.84,
                PostCode = "B29 5PY"
            };

            _customerRepositoryMock
                .Setup(x => x.GetCustomer(editRequest.Id)).Returns(_availableCustomers.FirstOrDefault(x => x.Id == editRequest.Id));


            var isValid = ModelValidate(editRequest);
            var result = _customerApplication.Edit(editRequest);

            isValid.ShouldBe(true);
            _customerRepositoryMock.Verify(x => x.GetCustomer(editRequest.Id), Times.Once);
            result.IsSucceeded.ShouldBe(true);
            result.Message.ShouldBe("Customer (Tom) Edited successfully!");

        }

        [Fact]
        public void Should_Return_Failed_Operation_Results_When_User_Does_Not_Exist()
        {
            var editRequest = new EditCustomer()
            {
                Id = 10,
                Name = "Tom",
                Age = 22,
                Height = 1.84,
                PostCode = "B29 5PY"
            };

            var isValid = ModelValidate(editRequest);
            var result = _customerApplication.Edit(editRequest);

            isValid.ShouldBe(true);
            _customerRepositoryMock.Verify(x => x.GetCustomer(editRequest.Id), Times.Once);
            result.IsSucceeded.ShouldBe(false);
            result.Message.ShouldBe("User cannot be found.");

        }

        [Fact]
        public void Should_Return_List_Of_Users_When_GetCustomers_Called()
        {

            var result = _customerApplication.GetCustomers();
            var viewModel = _availableCustomers.Select(x => new CustomerViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Age = x.Age,
                Height = x.Height,
                PostCode = x.PostCode,
            }).OrderBy(x=>x.Id).ToList();

            foreach (var item in result)
            {
                var isExist = viewModel.Exists(x => x.Id == item.Id &&
                                                    x.Name == item.Name && x.Age == item.Age &&
                                                    x.PostCode == item.PostCode);
                isExist.ShouldBe(true);
            }


            _customerRepositoryMock.Verify(x => x.GetCustomers(), Times.Once);
            
        }

        [Fact]
        public void Should_Return_EditCustomer_When_GetCustomers_Called()
        {
            var editCustomer = _availableCustomers.Select(x => new EditCustomer()
            {
                Id = x.Id,
                Name = x.Name,
                Age = x.Age,
                Height = x.Height,
                PostCode = x.PostCode,
            }).FirstOrDefault(x => x.Id == 1);
            var result = _customerApplication.GetDetails(1);

            result.Id.ShouldBe(editCustomer.Id);
            result.Name.ShouldBe(editCustomer.Name);
            result.PostCode.ShouldBe(editCustomer.PostCode);
            result.Age.ShouldBe(editCustomer.Age);
            result.Height.ShouldBe(editCustomer.Height);
            _customerRepositoryMock.Verify(x => x.GetCustomer(It.IsAny<long>()), Times.Once);

        }

        [Theory]
        [InlineData(true, 2)]
        [InlineData(false, 51)]
        public void Should_Return_Correct_Model_Validate_For_Name_Property(bool isValid, int count)
        {
            var name = new string('a', count);
            var createRequest = new CreateCustomer()
            {
                Name = name,
                Age = 22,
                Height = 1.84,
                PostCode = "B29 5PY"
            };
            var isModelValid = ModelValidate(createRequest);
            isModelValid.ShouldBe(isValid);

        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 150)]
        public void Should_Return_Correct_Model_Validate_For_Name_Age(bool isValid, int age)
        {
            var createRequest = new CreateCustomer()
            {
                Name = "Tom",
                Age = age,
                Height = 1.84,
                PostCode = "B29 5PY"
            };
            var isModelValid = ModelValidate(createRequest);
            isModelValid.ShouldBe(isValid);

        }

        [Theory]
        [InlineData(true, "a")]
        [InlineData(true, "a 2")]
        [InlineData(false, "?")]
        [InlineData(false, null)]
        public void Should_Return_Correct_Model_Validate_For_Name_PostCode(bool isValid, string? postCode)
        {
            var createRequest = new CreateCustomer()
            {
                Name = "Tom",
                Age = 20,
                Height = 1.84,
                PostCode = postCode
            };
            var isModelValid = ModelValidate(createRequest);
            isModelValid.ShouldBe(isValid);
        }

        [Theory]
        [InlineData(true, 1.62)]
        [InlineData(false, 1.622)]
        [InlineData(false, 2.62)]
        public void Should_Return_Correct_Model_Validate_For_Name_Height(bool isValid, double height)
        {
            var createRequest = new CreateCustomer()
            {
                Name = "Tom",
                Age = 20,
                Height = height,
                PostCode = "B29 5PY"
            };
            var isModelValid = ModelValidate(createRequest);
            isModelValid.ShouldBe(isValid);

        }
        private static bool ModelValidate<T>(T model) where T : new()
        {

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }
    }
}
