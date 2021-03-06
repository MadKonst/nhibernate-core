﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace NHibernate.Test.NHSpecificTest.NH2202
{
	using System.Linq;
	using Criterion;
	using NUnit.Framework;
	using System.Threading.Tasks;
	
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			base.OnSetUp();

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var emp = new Employee() {EmployeeId = 1, NationalId = 1000};
				emp.Addresses.Add(new EmployeeAddress() { Employee = emp, Type = "Postal" });
				emp.Addresses.Add(new EmployeeAddress() { Employee = emp, Type = "Shipping" });
				s.Save(emp);
				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from EmployeeAddress");
				tx.Commit();
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Employee");
				tx.Commit();
			}

			base.OnTearDown();
		}

		[Test]
		public async Task CanProjectEmployeeFromAddressUsingCriteriaAsync()
		{
			using (var s = OpenSession())
			{
				var employees = await (s.CreateCriteria<EmployeeAddress>("x3")
					.Add(Restrictions.Eq("Type", "Postal"))
					.SetProjection(Projections.Property("Employee"))
					.ListAsync<Employee>());

				Assert.That(employees.FirstOrDefault(), Is.InstanceOf<Employee>());
			}
		}
	}
}