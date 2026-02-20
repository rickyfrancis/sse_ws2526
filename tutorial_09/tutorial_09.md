## 🔽 Tutorial 09 🔒

[✉️ Subscribe](#)

**BPMN**

---

### Task 1

Prepare your working environment for this tutorial. You can do these exercises also on paper, but the available modeler will support your work. Choose therefore on either taking a local installation of the [Camunda](https://camunda.com/download/modeler/) Modeler or using the online tool [Cawemo](https://cawemo.com/), where you must sign up for free first.

If you need more detailed information than the lecture materials, you can find the specification of BPMN 2.0 and an example non-normative document at [https://www.omg.org/spec/BPMN/2.0/](https://www.omg.org/spec/BPMN/2.0/).

### Task 2

A fictitious factory prepares Yogurt to be sold for retail. Model the designated process as a collaboration diagram in BPMN 2.0 in the light of following consecutive sub-scenarios.

1. As the very first step a yogurt cup is picked from the cup stack. The cup is placed on the conveying belt. Then the cup is filled with yogurt. As soon as the cup is filled with yogurt the lid is placed on top and the cup is closed. The next step would be to bring the cup to the packaging conveying belt. The cup is then packaged in a box and the process ends.
2. The scenario stated at 2.1 lacks certain detail such as what is done once a box is full of cups. To deal with the concern, everytime after packaging the cup in a box a check is performed if the box is full or not. If full then the box is kept in the cold storage and a new box is retrieved from the stack of boxes. Else the process ends as stated before.
3. All the tasks in the process are handled by four separate workshops inside the factory. The workshops are "Conveying Workshop", "Filling Workshop", "Potting Workshop", "Packaging Workshop" respectively. The Conveying-Workshop is only responsible for picking the cups from the stack and placing them into the conveyor belt. On the other hand, the Filling-Workshop only fills yogurt into the cup and passes to the next workshop. The Potting-Workshop receives the filled cup with yogurt, puts a lid on it, closes the cup and brings the closed cup into the packaging conveyor belt. Finally, the Packaging-Workshop packages the cup in a box. It also checks if the current box is full or not. If it is full, then it keeps the box in the cold storage and gets a new box for the next cup. Either way, the process comes to an end.

### Task 3

Enhance the scenario described in "**Task 2**" according to following concerns.

1. **As stated before, the Filling-Workshop fills yogurt into the cup from the yogurt tank and passess to the next workshop. In addition to that it checks the volume of yogurt simultaneously. If the yogurt reserve reaches 1/3 of the max volume, an order is sent to the "Dairy Product Provider". The Dairy-Product-Provider receives the order, processes it and sends the requested amount of yogurt back. Once the yogurt arrives, the Yogurt tank is refilled and the sub-flow ends.**
2. **In addition to picking the cup and placing it onto the conveyor belt, the Conveying-Workshop also checks for empty rows in the Cup shelf. If at least one row is empty, an order is sent to the "Package Provider". The Package-Provider receives the order, process it and sends the asked number of cups back. The subflow waits in between. Once the cups arrive, they are sorted and the sub-flow ends.**
3. **Besides packaging the yogurt cup, Packaging-Workshop also checks the amount of boxes in stock. If the boxes are running low, an order is sent to Package-Provider and the subflow waits. Package-Provider on the other hand receives the box order, processes and sends the required amount of boxes back. As soon as the boxes arrive, the Packaging-Workshop adds them to the stack and the subflow ends.**

### Task 4

Model as a collaboration diagram in BPMN 2.0 the following visa application scenario. The participants are the _Visa Applicant_, the _Visa Service Center_ (VSC) and the _Immigration Office_ of the target country.

**<ins>The Scenario:</ins>**

First, the applicant submits an online visa application form to the VSC and receives a confirmation with an application ID. The applicant then performs the payment for the visa services and fixes an appointment with the VSC. Note: there should be no more than 30 days between the submission of the online visa application and appointment at the VSC, otherwise the online application is discarded by the system.

On the day of the appointment, the applicant shows up at the VSC. If not previously submitted for another visa application, the applicant submits then her biometric data. Moreover, the applicant submits all the printed forms, documents and her passport for consideration. The applicant has the possibility to pay for a courier service that delivers the documents when the process is completed or opt to pick them up in person.

Upon reception of the visa applications, the VSC performs some preliminary checks and forwards the application forms to the selected country’s immigration office. The immigration office carries out the necessary checks and decides the outcome of the visa application. At regular intervals, the VSC collects the responses from the immigration office of the target country. If the customer opted for picking up the documents in person, the visa application center notifies her via email. Otherwise, the documents are sent over courier to the applicant.

### Task 5

[BIMP](http://bimp.cs.ut.ee/simulator) is a free and simple simulator for BPMN business processes to conduct multi-instance simulations. Develop for each of your drawn diagram two different scenarios, one simple case of running everything at least once and one more advanced stressing scenario. Simulate those scenarios with BIMP. What are your observations?
