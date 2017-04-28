
import java.util.ArrayList;
import java.util.List;






public class ttt {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		List<Addressbook.Person.PhoneNumber> phones= new ArrayList<>();
		Addressbook.Person.PhoneNumber phone = Addressbook.Person.PhoneNumber.newBuilder().setNumber("13429632852").setType(Addressbook.Person.PhoneType.HOME).build();
		phones.add(phone);

		List<Addressbook.Person> persons = new ArrayList<>();
		Addressbook.Person p = Addressbook.Person.newBuilder().setEmail("512614226@qq.com").setId(8).setName("Joe").addAllPhone(phones).build();
		persons.add(p);
		
		Addressbook.AddressBook book = Addressbook.AddressBook.newBuilder().addAllPerson(persons).build();	
		System.out.println(book);
		
		Msg.Person testmsg = Msg.Person.newBuilder().setEmail("123@163.com").setName("hq").setId(110).build();
		System.out.print(testmsg);
		

	
		
/*      book.getPerson(0);
      List<Addressbook.Person> list =  book.getPersonList();
      Addressbook.Person person = list.get(1);*/


	}

}
