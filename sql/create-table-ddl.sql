-- creates a sample relational car dealer table
CREATE TABLE ADMIN.DEALER 
    ( 
     NAME    VARCHAR2 (100) , 
     ADDRESS VARCHAR2 (100) , 
     MAKE    VARCHAR2 (20) 
    ) 
    TABLESPACE DATA 
    LOGGING 
;