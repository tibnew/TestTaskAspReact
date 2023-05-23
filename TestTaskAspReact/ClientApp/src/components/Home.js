import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { users: [], loading: true };
    }

    componentDidMount() {
        this.populateUsersData();
    }

    static renderUsersTable(users) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Avatar</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user =>
                        <tr key={user.name}>
                            <td>{user.name}</td>
                            <td><img src={user.profilePictureUrl} width="50" height="50"></img></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderUsersTable(this.state.users);

        return (
            <div>
                <h1 id="tableLabel">Users</h1>
                {contents}
            </div>
        );
    }

    async populateUsersData() {
        const token = await authService.getAccessToken();
        const response = await fetch('home', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ users: data, loading: false });
    }
}
