drop table if exists Users;
drop table if exists ExamExercises;
drop table if exists Exams;
drop table if exists QuizExercises;
drop table if exists Quizzes;
drop table if exists AssociationPairs;
drop table if exists AssociationExercises;
drop table if exists FillInTheBlankAnswers;
drop table if exists FillInTheBlankExercises;
drop table if exists FlashcardExercises;
drop table if exists MultipleChoiceOptions;
drop table if exists MultipleChoiceExercises;
drop table if exists Exercises;
drop table if exists Sections;
drop table if exists Roadmaps;
drop table if exists Difficulties;

-- Create Difficulty enum table
CREATE TABLE Difficulties (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
);

-- Insert difficulty values
INSERT INTO Difficulties (Id, Name) VALUES
(1, 'Easy'),
(2, 'Medium'),
(3, 'Hard');

-- Create Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    NumberOfCompletedSections INT DEFAULT 0,
    NumberOfCompletedQuizzesInSection INT DEFAULT 0
);

-- Create Roadmap table
CREATE TABLE Roadmaps (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

-- Create Sections table
CREATE TABLE Sections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SubjectId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Description VARCHAR(500),
    RoadmapId INT NOT NULL,
    OrderNumber INT NOT NULL,
    FOREIGN KEY (RoadmapId) REFERENCES Roadmaps(Id),
);

-- Create Exercises table (base table for exercise types)
CREATE TABLE Exercises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type VARCHAR(50) NOT NULL, -- 'MultipleChoice', 'FillInTheBlank', 'Association', 'Flashcard'
    DifficultyId INT NOT NULL,
    Question VARCHAR(500) NOT NULL,
    FOREIGN KEY (DifficultyId) REFERENCES Difficulties(Id)
);

-- Create MultipleChoiceExercises table
CREATE TABLE MultipleChoiceExercises (
    ExerciseId INT PRIMARY KEY,
    CorrectAnswer VARCHAR(500) NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE
);

-- Create MultipleChoiceOptions table
CREATE TABLE MultipleChoiceOptions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ExerciseId INT NOT NULL,
    OptionText VARCHAR(500) NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES MultipleChoiceExercises(ExerciseId) ON DELETE CASCADE
);

-- Create FillInTheBlankExercises table
CREATE TABLE FillInTheBlankExercises (
    ExerciseId INT PRIMARY KEY,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE
);

-- Create FillInTheBlankAnswers table
CREATE TABLE FillInTheBlankAnswers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ExerciseId INT NOT NULL,
    CorrectAnswer VARCHAR(100) NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES FillInTheBlankExercises(ExerciseId) ON DELETE CASCADE,
);

-- Create AssociationExercises table
CREATE TABLE AssociationExercises (
    ExerciseId INT PRIMARY KEY,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE
);

-- Create AssociationPairs table
CREATE TABLE AssociationPairs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ExerciseId INT NOT NULL,
    FirstAnswer VARCHAR(255) NOT NULL,
    SecondAnswer VARCHAR(255) NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES AssociationExercises(ExerciseId) ON DELETE CASCADE
);

-- Create Quizzes table
CREATE TABLE Quizzes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SectionId INT DEFAULT NULL,
    OrderNumber INT DEFAULT NULL,
    FOREIGN KEY (SectionId) REFERENCES Sections(Id) ON DELETE SET NULL
);

-- Create QuizExercises table (junction table between Quizzes and Exercises)
CREATE TABLE QuizExercises (
    QuizId INT NOT NULL,
    ExerciseId INT NOT NULL,
    PRIMARY KEY (QuizId, ExerciseId),
    FOREIGN KEY (QuizId) REFERENCES Quizzes(Id) ON DELETE CASCADE,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE,
);

-- Create Exams table
CREATE TABLE Exams (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SectionId INT DEFAULT NULL,
    FOREIGN KEY (SectionId) REFERENCES Sections(Id) ON DELETE SET NULL
);

-- Create ExamExercises table (junction table between Exams and Exercises)
CREATE TABLE ExamExercises (
    ExamId INT NOT NULL,
    ExerciseId INT NOT NULL,
    PRIMARY KEY (ExamId, ExerciseId),
    FOREIGN KEY (ExamId) REFERENCES Exams(Id) ON DELETE CASCADE,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE,
);

-- Create FlashcardExercises table
CREATE TABLE FlashcardExercises (
    ExerciseId INT PRIMARY KEY,
    Answer VARCHAR(100) NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE
);
