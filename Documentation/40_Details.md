Details {#details}
================

The section of the docu explains a few important details of DOF with more detials.

\section details_conditions Conditions

DOF supports the use of conditions. (e.g for the \ref TableElementConditionEffect). A simple condition might look like this:
~~~~~~~~~~~~~~~~~~
(S48=1)
~~~~~~~~~~~~~~~~~~
Or a little more complex like this
~~~~~~~~~~~~~~~~~~
(S48=1 and (S46=1 or W16=1))
~~~~~~~~~~~~~~~~~~

Conditions are evaluated at runtime using the <a href="http://flee.codeplex.com/">Fast Lightweight Expression Evaluator (FLEE)</a>, which means that alll conditions have the follow the syntag for conditions in FLEE.

When DOF evaluates a condition it allows access to all table elements. In conditions table elements are specified by the a letter according to the table element type enum (e.g. S=solenoid, W=switch, L=lamp, E=eEM table event, but more letters ecist) and the number for the table element (e.g. S48 for solenoid number 48). Table elements can be used in conditions as follows:

* __(S48=1)__: This is the normal way to use table elements in coditions. This reference to table table element will always reference the value of the table element.
* __(S48.ValueHasChanged(1000))__: Is another way to access a table element. Like this you can adress all properties and methods of the table lement.

For more details regarding the the expression language for conditions please read: http://flee.codeplex.com/wikipage?title=LanguageReference

