import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            currentUser: undefined,
            users: [],
            loading: true
        };
    }

    componentDidMount() {
        this.populateUsersData();
    }

    async handleMakeAdminOnClick(index) {
        const token = await authService.getAccessToken();
        const response = await fetch('users/' + this.state.users[index].id + '/roles/1', {
            method: 'POST',
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        if (this.state.users[index].id == this.state.currentUser.id) {
            window.location.reload(false);
        } else {
            const newUsers = [...this.state.users];
            newUsers[index].isAdmin = true;
            this.setState({ users: newUsers });
        }
    }

    async handleDeleteOnClick(index) {
        const newUsers = [...this.state.users];
        const token = await authService.getAccessToken();
        const response = await fetch('users/' + newUsers[index].id, {
            method: 'DELETE',
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        newUsers.splice(index, 1);
        this.setState({ users: newUsers });
    }

    renderUsersTable(users) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Avatar</th>
                        <th>IsAdmin</th>
                        <th>Make admin</th>
                        {this.state.currentUser.isAdmin &&
                            <th>IsCurrentUser</th>
                        }
                        {this.state.currentUser.isAdmin &&
                        <th>Delete</th>
                        }
                    </tr>
                </thead>
                <tbody>
                    {users.map((user, index) =>
                        <tr key={user.id}>
                            <td>{user.name}</td>
                            <td><img src={user.profilePictureUrl} width="50" height="50"></img></td>
                            <td>{user.isAdmin ? 'Yes' : 'No'}</td>
                            <td><button onClick={async () => await this.handleMakeAdminOnClick(index)} >Make admin</button></td>
                            {
                                this.state.currentUser.isAdmin &&
                                <td>{user.id == this.state.currentUser.id ? 'Yes' : 'No'}</td>
                                
                            }
                            {
                                this.state.currentUser.isAdmin &&
                                <td>{
                                    
                                        user.id != this.state.currentUser.id &&
                                        <button onClick={async () => await this.handleDeleteOnClick(index)} >Delete</button>

                                    
                                    }</td>
                            }
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderUsersTable(this.state.users);

        return (
            <div>
                <h1 id="tableLabel">Users</h1>
                {contents}
            </div>
        );
    }

    async populateUsersData() {
        const token = await authService.getAccessToken();
        const response = await fetch('users', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();

        const getCurrentUserResponse = await fetch('users/current', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const currentUser = await getCurrentUserResponse.json();

        this.setState({ currentUser: currentUser, users: data, loading: false });
    }
}
