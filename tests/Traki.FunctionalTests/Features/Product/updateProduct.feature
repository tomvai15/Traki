Feature: Update product

 
Scenario: Update product with valid fields
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    When I open edit product page
    And update product name
    Then product name should be updated


 
Scenario: Update product with invalid fields
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    When I open edit product page
    And update product name with invalid characters
    Then I should not be allowed to update product