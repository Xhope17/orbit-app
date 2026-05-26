# Orbit — Social Platform

## Overview

Orbit is a social platform inspired by X/Twitter, focused on real-time interaction, communities, private messaging, and hashtag-based content discovery.

The platform allows users to:
- Create profiles
- Follow other users
- Publish posts
- Use hashtags
- Join communities
- Send private messages
- Interact through a personalized social feed

The architecture is designed to be scalable, modular, and backend-oriented, supporting future expansion into real-time systems, recommendation engines, notifications, and distributed services.

---

# Main Features

## User System

Users can create accounts and personalize their profiles.

Each user contains:
- Username
- Unique user prefix/tag
- Display name
- Biography
- Profile image
- Banner image
- Verification status
- Privacy configuration

The system uses globally unique identifiers (UUIDs) as primary keys.

---

# Follow System

Users can follow other users.

The follow system stores:
- Follower user ID
- Followed user ID

Additionally:
- Relationships are normalized using a symmetric unique pair system:
  - `user_low`
  - `user_high`

This prevents duplicate relationships between the same pair of users.

Example:
- A follows B
- B follows A

Only one normalized pair exists internally.

---

# Ban System

Users can block other users.

The ban list uses the same normalized relationship logic as follows to avoid duplicate records.

Blocked users:
- Cannot interact
- Cannot send messages
- Cannot access certain content depending on future rules

---

# Posts

Users can publish posts.

Posts support:
- Text content
- Replies
- Reposts
- Community-only visibility
- Public visibility
- Followers-only visibility

Each post belongs to:
- A user
- Optionally a community

Future extensions:
- Media uploads
- Polls
- Rich embeds
- AI moderation
- Recommendation algorithms

---

# Hashtag System

Posts may contain hashtags.

The backend automatically:
1. Extracts hashtags from post content
2. Normalizes them
3. Stores them in the database
4. Creates relationships between posts and hashtags

This enables:
- Trending systems
- Search by hashtag
- Content indexing
- Recommendation systems

---

# Communities

The platform supports communities/groups.

Communities contain:
- Name
- Description
- Owner
- Members
- Roles

Roles:
- Member
- Moderator
- Administrator

Community posts are visible only to members of the corresponding community.

This creates semi-private social spaces inside the platform.

---

# Chat System

Users can communicate through private chats.

A chat:
- Exists between two users
- Is unique per user pair
- Stores messages independently

Messages contain:
- Sender ID
- Chat ID
- Message content
- Timestamp

Future improvements:
- Read receipts
- Typing indicators
- Real-time communication
- Media attachments
- Voice messages

---

# Visibility Rules

The platform supports multiple visibility levels:

| Visibility | Description |
|---|---|
| Public | Visible to everyone |
| Followers | Visible only to followers |
| Community | Visible only to community members |

Authorization checks are enforced by the backend.

---

# Database Design

The project uses SQL Server as the relational database engine.

Main tables:
- Users
- Follows
- Bans
- Posts
- Hashtags
- PostHashtags
- Communities
- CommunityMembers
- Chats
- Messages

The database design prioritizes:
- Data integrity
- Relationship normalization
- Scalability
- Query performance
- Extensibility

---

# Backend Architecture

The backend is designed using Clean Architecture principles.

Layers:
- Domain
- Application
- Infrastructure
- Presentation/API

Benefits:
- Separation of concerns
- Testability
- Scalability
- Maintainability

---

# Planned Technologies

## Backend
- .NET
- ASP.NET Core
- SQL Server
- Dapper or Entity Framework Core

## Infrastructure
- Redis (caching/feed optimization)
- SignalR (real-time communication)
- Cloud storage for media
- Background workers

## Security
- JWT authentication
- Role-based authorization
- Secure password hashing
- Rate limiting
- Validation pipelines

---

# Future Features

Planned future modules:
- Likes
- Notifications
- Bookmarks
- Media uploads
- Feed recommendation engine
- Trending system
- Search engine
- User mentions
- AI moderation
- Analytics
- Real-time events
- Distributed microservices

---

# Scalability Vision

The system is designed with future scalability in mind.

Possible future architecture:
- API Gateway
- Microservices
- Event-driven communication
- Distributed caching
- Queue systems
- Search indexing
- CDN integration

The project aims to evolve from a monolithic MVP into a scalable distributed social platform.