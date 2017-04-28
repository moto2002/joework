/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import com.example.tutorial.AddressBookProtos;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Administrator
 */
public class Test {

    public static void main(String[] args) 
    {
        List<AddressBookProtos.Person.PhoneNumber> phones = new ArrayList();
        AddressBookProtos.Person.PhoneNumber phone = AddressBookProtos.Person.PhoneNumber.newBuilder().setNumber("12345").setType(AddressBookProtos.Person.PhoneType.WORK).build();
        phones.add(phone);
        
        List<AddressBookProtos.Person> persons = new ArrayList();
        AddressBookProtos.Person person = AddressBookProtos.Person.newBuilder().setEmail("11@aa.com").setId(11).setName("22ee").addAllPhone(phones).build();
        persons.add(person);
        
        AddressBookProtos.AddressBook book = AddressBookProtos.AddressBook.newBuilder().addAllPerson(persons).build();
        System.out.println(book);

//        book.getPerson(0);
//       List<AddressBookProtos.Person> list =  book.getPersonList();
//      person = list.get(1);
    }
}
